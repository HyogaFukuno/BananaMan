using UnityEngine.AddressableAssets;

namespace BananaMan.Audios.Core;

public interface IAudioTable
{
    AssetReference? TitleBgmReference { get; }
}