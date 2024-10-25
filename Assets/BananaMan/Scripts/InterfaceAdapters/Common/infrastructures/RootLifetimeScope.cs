using BananaMan.Audios.Core;
using BananaMan.Audios.Infrastructures;
using BananaMan.Common.Core;
using BananaMan.Common.Infrastructures;
using Microsoft.Extensions.Logging;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZLogger.Unity;

namespace BananaMan.Common.infrastructures
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] LogLevel minimumLevel;
        
        [SerializeField] GameOptions? gameOptions;
        
        [SerializeField] AudioTableScriptableObject? audioTable;
        [SerializeField] GameObject? audioContainer;
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
            builder.Register<AudioPlayerServiceImpl>(Lifetime.Singleton)
                .WithParameter(audioPlayerSettings)
                .WithParameter(audioContainer)
                .As<IAudioPlayerService>();
            builder.RegisterInstance(audioTable)
                .As<IAudioTable>();
        }
    }
}