using System.Threading;
using BananaMan.Common.ViewModels;
using BananaMan.Titles.ViewModels;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VContainer.Unity;
using ZLogger;

namespace BananaMan.Titles.Presenter;

public sealed class TitlePresenter : IAsyncStartable
{
    readonly ILogger<TitlePresenter> logger;
    readonly MainViewModel mainViewModel;

    public TitlePresenter(ILogger<TitlePresenter> logger, MainViewModel mainViewModel)
    {
        this.logger = logger;
        this.mainViewModel = mainViewModel;
    }

    public async UniTask StartAsync(CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.StartAsync");

        mainViewModel.AddView();
        await mainViewModel.InitializeAsync(ct);
        await mainViewModel.OpenWithoutAddAsync(SceneTransitionState.Next, ct);
    }
}