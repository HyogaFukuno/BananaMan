using System.Threading;
using BananaMan.Audios.Core;
using BananaMan.Common.ViewModels;
using BananaMan.Titles.Core;
using BananaMan.Titles.ViewModels;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlasticGui;
using VContainer.Unity;
using ZLogger;

namespace BananaMan.Titles.Presenter;

public sealed class TitlePresenter : IAsyncStartable
{
    readonly ILogger<TitlePresenter> logger;
    readonly TitleModel core;
    readonly AudioPlayer audioPlayer;
    readonly IAudioTable audioTable;
    readonly MainViewModel mainViewModel;
    readonly OptionsViewModel optionsViewModel;

    public TitlePresenter(ILogger<TitlePresenter> logger,
                          TitleModel core,
                          AudioPlayer audioPlayer,
                          IAudioTable audioTable,
                          MainViewModel mainViewModel,
                          OptionsViewModel optionsViewModel)
    {
        this.logger = logger;
        this.core = core;
        this.audioPlayer = audioPlayer;
        this.audioTable = audioTable;
        this.mainViewModel = mainViewModel;
        this.optionsViewModel = optionsViewModel;
    }

    public async UniTask StartAsync(CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.StartAsync");
        
        mainViewModel.AddView(addedTransition: true);

        await audioPlayer.InitializeAsync(ct);
        
        await mainViewModel.InitializeAsync(core.CurrentViewType, ct);
        await optionsViewModel.InitializeAsync(core.CurrentViewType, ct);
        await UniTask.WaitForSeconds(1.0f, cancellationToken: ct);

        mainViewModel.OpenOptionsAsync = OpenOptionsAsync;
        optionsViewModel.CloseOptionsAsync = CloseOptionsAsync;
        
        await UniTask.WhenAll(
            mainViewModel.OpenWithoutAddAsync(SceneTransitionState.Next, ct),
            audioPlayer.PlayBgmAsync(audioTable.TitleBgmReference, ct));
    }
    
    

    async UniTask OpenOptionsAsync(SceneTransitionState state, CancellationToken ct)
    {
        core.CurrentViewType.Value = TitleViewType.Options;
        await optionsViewModel.OpenAsync(state, ct);
    }

    async UniTask CloseOptionsAsync(SceneTransitionState state, CancellationToken ct)
    {
        core.CurrentViewType.Value = TitleViewType.Main;
        await optionsViewModel.CloseAsync(state, ct);
        mainViewModel.PreOpen();
    }
}