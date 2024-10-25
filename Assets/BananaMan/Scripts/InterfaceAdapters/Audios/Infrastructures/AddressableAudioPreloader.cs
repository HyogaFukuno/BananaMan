using System.Threading;
using BananaMan.Audios.Core;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnityEngine.AddressableAssets;

namespace BananaMan.Audios.Infrastructures;

public sealed class AddressableAudioPreloader : IAudioPreloader
{
    readonly ILogger<AddressableAudioPreloader> logger;
    readonly IAudioLoader loader;
    readonly AssetReference?[] preloadReferences;

    public AddressableAudioPreloader(ILogger<AddressableAudioPreloader> logger,
                                     IAudioLoader loader,
                                     AssetReference?[] preloadReferences)
    {
        this.logger = logger;
        this.loader = loader;
        this.preloadReferences = preloadReferences;
    }

    public async UniTask PreloadAsync(CancellationToken ct)
    {
        foreach (var reference in preloadReferences)
        {
            await loader.LoadAsync(reference, ct);
        }
    }
}