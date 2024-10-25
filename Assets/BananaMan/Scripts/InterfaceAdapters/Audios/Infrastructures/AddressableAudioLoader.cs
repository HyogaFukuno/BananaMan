using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZLogger;

namespace BananaMan.Audios.Infrastructures;

public sealed class AddressableAudioLoader : IAudioLoader, IDisposable
{
    class AudioClipDisposable : IDisposable
    {
        readonly IDisposable? disposable;
        
        public AudioClip? Clip { get; }
        
        public AudioClipDisposable(AudioClip? clip, AssetReference? reference)
        {
            Clip = clip;
            disposable = Disposable.Create(reference, static x => x?.ReleaseAsset());
        }
        
        public void Dispose() => disposable?.Dispose();
    }

    readonly Dictionary<string, AudioClipDisposable?> cache = new(capacity: 24);
    readonly ILogger<AddressableAudioLoader> logger;

    public AddressableAudioLoader(ILogger<AddressableAudioLoader> logger)
    {
        this.logger = logger;
    }

    public async UniTask<AudioClip?> LoadAsync(AssetReference? reference, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}");
        
        if (reference is null)
        {
            return null;
        }

        AudioClip? clip;
        if (cache.TryGetValue(reference.AssetGUID, out var value))
        {
            clip = value?.Clip;
            await UniTask.WaitUntil(clip, x => x is not null, cancellationToken: ct);
        }
        else
        {
            cache.Add(reference.AssetGUID, null);
            clip = await reference.LoadAssetAsync<AudioClip>().WithCancellation(ct);
            
            logger.ZLogTrace($"before clip.loadState: {clip.loadState}");
            clip.LoadAudioData();
            await UniTask.WaitUntil(clip, 
                static x => x.loadState == AudioDataLoadState.Loaded, cancellationToken: ct);
            logger.ZLogTrace($"after clip.loadState: {clip.loadState}");
            
            cache[reference.AssetGUID] = new AudioClipDisposable(clip, reference);
        }
        
        return clip;
    }

    public async UniTask UnloadAsync(AssetReference? reference, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}");

        if (reference is null || !cache.TryGetValue(reference.AssetGUID, out var value))
        {
            return;
        }
        
        value?.Clip?.UnloadAudioData();
        value?.Dispose();
        cache.Remove(reference.AssetGUID);
        await UniTask.Yield(ct);
    }

    public void Dispose()
    {
        foreach (var value in cache.Values)
        {
            value?.Dispose();
        }
        cache.Clear();
    }
}