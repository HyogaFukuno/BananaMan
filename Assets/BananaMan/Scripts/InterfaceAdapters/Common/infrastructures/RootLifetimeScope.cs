using System.IO;
using BananaMan.ApplicationBusinessRules.Interfaces;
using BananaMan.Audios.Core;
using BananaMan.Audios.Infrastructures;
using BananaMan.Common.Core;
using BananaMan.Common.Infrastructures;
using BananaMan.EnterpriseBusinessRules;
using BananaMan.Frameworks;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;
using ZLogger.Unity;

namespace BananaMan.Common.infrastructures
{
    public class RootLifetimeScope : LifetimeScope
    {
        static readonly string MasterPath = Path.Combine(Application.streamingAssetsPath, "master_data");
        
        [SerializeField] LogLevel minimumLevel;
        
        [SerializeField] GameOptions? gameOptions;
        
        [SerializeField] AudioTableScriptableObject? audioTable;
        [SerializeField] GameObject? audioContainer;
        [SerializeField] AssetReference?[]? preloadReferences;
        [SerializeField] AudioPlayerSettings audioPlayerSettings;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(LoggerFactory.Create(logger =>
            {
                logger.SetMinimumLevel(minimumLevel);
                logger.AddZLoggerUnityDebug();
            }));

            builder.Register(typeof(Logger<>), Lifetime.Singleton).As(typeof(ILogger<>));

            builder.RegisterInstance(gameOptions)
                .As<IReadOnlyGameOptions>();
            
            builder.Register<AudioPlayer>(Lifetime.Singleton);
            builder.Register<AddressableAudioLoader>(Lifetime.Singleton)
                .As<IAudioLoader>();
            builder.Register<AddressableAudioPreloader>(Lifetime.Singleton)
                .WithParameter(preloadReferences)
                .As<IAudioPreloader>();
            builder.Register<AudioPlayerServiceImpl>(Lifetime.Singleton)
                .WithParameter(audioPlayerSettings)
                .WithParameter(audioContainer)
                .As<IAudioPlayerService>();
            builder.RegisterInstance(audioTable)
                .As<IAudioTable>();
            
            builder.Register<JsonAsyncDataStore<SaveData[]>>(Lifetime.Singleton)
                .WithParameter(Path.Combine(MasterPath, "save_data.json"))
                .As<IAsyncDataStore<SaveData[]>>();
            builder.Register<SaveDataRepository>(Lifetime.Singleton)
                .As<IAsyncRepository<SaveData, SaveDataId>>();
        }
    }
}