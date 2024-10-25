using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BananaMan.Audios.Infrastructures;

public interface IAudioLoader
{
    UniTask<AudioClip?> LoadAsync(AssetReference? reference, CancellationToken ct);
    UniTask UnloadAsync(AssetReference? reference, CancellationToken ct);
}