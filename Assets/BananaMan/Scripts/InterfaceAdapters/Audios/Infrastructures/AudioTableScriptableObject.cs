using BananaMan.Audios.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BananaMan.Audios.Infrastructures
{
    [CreateAssetMenu(fileName = "New AudioTable", menuName = "Banana Man/Audio Table")]
    public sealed class AudioTableScriptableObject : ScriptableObject, IAudioTable
    {
        [SerializeField] AssetReference? titleBgmReference;
        
        public AssetReference? TitleBgmReference => titleBgmReference;
    }
}