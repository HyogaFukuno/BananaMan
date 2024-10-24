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
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(LoggerFactory.Create(logger =>
            {
                logger.SetMinimumLevel(minimumLevel);
                logger.AddZLoggerUnityDebug();
            }));

            builder.Register(typeof(Logger<>), Lifetime.Singleton).As(typeof(ILogger<>));
        }
    }
}