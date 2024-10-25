using System.Collections.Generic;
using System.Linq;
using BananaMan.Common.Core;
using FastEnumUtility;
using R3;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BananaMan.Common.Infrastructures
{
    [CreateAssetMenu(fileName = "GameOptions", menuName = "Banana Man/Game Options")]
    public sealed class GameOptions : ScriptableObject, IReadOnlyGameOptions
    {
        [SerializeField, CreateProperty] GraphicsSettingsType graphicsSettings;
        [SerializeField, CreateProperty] bool soundEnabled;
        [SerializeField, CreateProperty] float bgmVolume;
        [SerializeField, CreateProperty] float seVolume;

        [CreateProperty(ReadOnly = true)]
        public List<string?> GraphicsSettingsTypeNames { get; }
            = FastEnum.GetValues<GraphicsSettingsType>()
                .Select(static x => x.GetLabel())
                .ToList();

        public Observable<GraphicsSettingsType> GraphicsSettings
            => Observable.EveryValueChanged(this, static x => x.graphicsSettings);
        
        public Observable<bool> SoundEnabled
            => Observable.EveryValueChanged(this, static x => x.soundEnabled);
        
        public Observable<float> BgmVolume
            => Observable.EveryValueChanged(this, static x => x.bgmVolume);
        
        public Observable<float> SeVolume
            => Observable.EveryValueChanged(this, static x => x.seVolume);


#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void RegisterConverters()
        {
            RegisterGraphicsSettingsTypeConverter();
            RegisterSerializableReactivePropertyConverter();
        }

        static void RegisterGraphicsSettingsTypeConverter()
        {
            // コンバーターを作成する
            var group = new ConverterGroup("Integer to GraphicsSettingsType");

            // 数値（Index）をGraphicsSettingsTypeに変換する処理を登録する
            group.AddConverter((ref GraphicsSettingsType v) => (int)v);
            group.AddConverter((ref int v) => (GraphicsSettingsType)v);

            // コンバーターグループに作成したコンバーターを登録することでUI Builderで使用可能になる
            ConverterGroups.RegisterConverterGroup(group);
        }

        static void RegisterSerializableReactivePropertyConverter()
        {
            // // コンバーターを作成する
            // var group = new ConverterGroup("Float to SerializableReactiveProperty<float>");
            //
            // // 数値（Index）をGraphicsSettingsTypeに変換する処理を登録する
            // group.AddConverter((ref SerializableReactiveProperty<float> v) => v.CurrentValue);
            // group.AddConverter((ref float v) => SerializableReactiveProperty.Create(v));
            //
            // // コンバーターグループに作成したコンバーターを登録することでUI Builderで使用可能になる
            // ConverterGroups.RegisterConverterGroup(group);
        }
    }
}