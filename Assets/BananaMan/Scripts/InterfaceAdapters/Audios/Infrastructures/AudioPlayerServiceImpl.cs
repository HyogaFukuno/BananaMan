using System;
using System.Collections.Generic;
using System.Linq.Extensions;
using System.Threading;
using BananaMan.Audios.Core;
using BananaMan.Common.Core;
using Cysharp.Threading.Tasks;
using LitMotion;
using Microsoft.Extensions.Logging;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using ZLogger;

namespace BananaMan.Audios.Infrastructures;

[Serializable]
public struct AudioPlayerSettings
{
    public int maxBgmSources;
    public int maxSeSources;
    public AudioMixer audioMixer;
    public AudioMixerGroup bgmAudioMixerGroup;
    public AudioMixerGroup seAudioMixerGroup;
    public AnimationCurve volumeCurve;
}

public sealed class AudioPlayerServiceImpl : IAudioPlayerService, IDisposable
{
    const float AttenuationMinimumVolume = -80.0f;
    const float AttenuationMaximumVolume = 0.0f;
    
    readonly ILogger<AudioPlayerServiceImpl> logger;
    readonly IAudioLoader loader;
    readonly AudioPlayerSettings settings;
    readonly GameObject gameObject;
    readonly IReadOnlyGameOptions gameOptions;
    List<AudioSource>? bgmSources;
    List<AudioSource>? seSources;
    bool initialized;
    bool audioEnabled;
    DisposableBag bag;

    public AudioPlayerServiceImpl(ILogger<AudioPlayerServiceImpl> logger,
                                  IAudioLoader loader,
                                  AudioPlayerSettings settings,
                                  GameObject gameObject,
                                  IReadOnlyGameOptions gameOptions)
    {
        this.logger = logger;
        this.loader = loader;
        this.settings = settings;
        this.gameObject = gameObject;
        this.gameOptions = gameOptions;
    }
    
    public async UniTask InitializeAsync(CancellationToken ct)
    {
        if (initialized)
        {
            return;
        }

        bgmSources = new List<AudioSource>(capacity: settings.maxBgmSources);
        for (var i = 0; i < settings.maxBgmSources; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = settings.bgmAudioMixerGroup;
            bgmSources.Add(source);
        }

        await UniTask.Yield(ct);
        
        seSources = new List<AudioSource>(capacity: settings.maxSeSources);
        for (var i = 0; i < settings.maxSeSources; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = settings.seAudioMixerGroup;
            seSources.Add(source);
        }
        
        await UniTask.Yield(ct);
        
        gameOptions.SoundEnabled.Subscribe(OnChangeAudioEnabled).AddTo(ref bag);
        gameOptions.BgmVolume.Subscribe(OnChangeBgmVolume).AddTo(ref bag);
        gameOptions.SeVolume.Subscribe(OnChangeSeVolume).AddTo(ref bag);

        initialized = true;
    }

    public async UniTask PlayBgmAsync(AssetReference? reference, float volume, CancellationToken ct)
    {
        if (!initialized)
        {
            logger.ZLogWarning($"{GetType().Name} has not initialized.");
            return;
        }
        
        if (!audioEnabled)
        {
            logger.ZLogTrace($"Audio is not enabled.");
            return;
        }

        var clip = await loader.LoadAsync(reference, ct);

        var emptySource = bgmSources?.Find(static source => !source.isPlaying);
        emptySource?.SetClip(clip);
        emptySource?.SetVolume(volume);
        emptySource?.Play();
    }

    public async UniTask PlaySeAsync(AssetReference? reference, float volume, CancellationToken ct)
    {
        if (!initialized)
        {
            logger.ZLogWarning($"{GetType().Name} has not initialized.");
            return;
        }

        if (!audioEnabled)
        {
            logger.ZLogTrace($"Audio is not enabled.");
            return;
        }

        var clip = await loader.LoadAsync(reference, ct);

        var emptySource = bgmSources?.Find(static source => !source.isPlaying);
        emptySource?.SetVolume(volume);
        emptySource?.PlayOneShot(clip);
    }

    public async UniTask StopBgmAsync(AssetReference? reference, float duration, CancellationToken ct)
    {
        if (!initialized)
        {
            logger.ZLogWarning($"{GetType().Name} has not initialized.");
            return;
        }

        if (!audioEnabled)
        {
            logger.ZLogTrace($"Audio is not enabled.");
            return;
        }
        
        var clip = await loader.LoadAsync(reference, ct);

        var playedSource = bgmSources?.Find(source => source.clip == clip);

        if (duration > 0.0f)
        {
            var current = playedSource?.volume;
            await LMotion.Create(current!.Value, 0.0f, duration)
                .Bind(x => playedSource?.SetVolume(x))
                .ToUniTask(ct);
        }

        playedSource?.Stop();
    }


    public void Dispose()
    {
        bgmSources?.Clear();
        seSources?.Clear();
        bag.Dispose();
    }

    void OnChangeAudioEnabled(bool enabled)
    {
        bgmSources?.ForEach(enabled, static (x, y) => x.enabled = y);
        seSources?.ForEach(enabled, static (x, y) => x.enabled = y);
        audioEnabled = enabled;
    }
    
    void OnChangeBgmVolume(float volume) => settings.audioMixer.SetFloat("Bgm_Volume", Mathf.Lerp(
        AttenuationMinimumVolume,
        AttenuationMaximumVolume,
        settings.volumeCurve.Evaluate(volume)));
    
    void OnChangeSeVolume(float volume) => settings.audioMixer.SetFloat("Se_Volume", Mathf.Lerp(
        AttenuationMinimumVolume,
        AttenuationMaximumVolume,
        settings.volumeCurve.Evaluate(volume)));
}


internal static class AudioSourceExtensions
{
    public static void SetClip(this AudioSource source, AudioClip? clip)
    {
        source.clip = clip;
    }

    public static void SetVolume(this AudioSource source, float volume)
    {
        source.volume = volume;
    }
}