using System.Threading;
using BananaMan.ApplicationBusinessRules.UseCases;
using BananaMan.Audios.Core;
using BananaMan.Common.ViewModels;
using BananaMan.Titles.Core;
using BananaMan.Titles.ViewModels;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VContainer.Unity;
using ZLogger;

namespace BananaMan.Titles.Presenter;

public sealed class TitlePresenter : IAsyncStartable
{
    readonly ILogger<TitlePresenter> logger;
    readonly TitleModel core;
    readonly AudioPlayer audioPlayer;
    readonly IAudioPreloader audioPreloader;
    readonly IAudioTable audioTable;
    readonly FindSaveDataUseCase findSaveDataUseCase;
    readonly MainViewModel mainViewModel;
    readonly ContinueViewModel continueViewModel;
    readonly OptionsViewModel optionsViewModel;

    public TitlePresenter(ILogger<TitlePresenter> logger,
                          TitleModel core,
                          AudioPlayer audioPlayer,
                          IAudioPreloader audioPreloader,
                          IAudioTable audioTable,
                          FindSaveDataUseCase findSaveDataUseCase,
                          MainViewModel mainViewModel,
                          ContinueViewModel continueViewModel,
                          OptionsViewModel optionsViewModel)
    {
        this.logger = logger;
        this.core = core;
        this.audioPlayer = audioPlayer;
        this.audioPreloader = audioPreloader;
        this.audioTable = audioTable;
        this.findSaveDataUseCase = findSaveDataUseCase;
        this.mainViewModel = mainViewModel;
        this.continueViewModel = continueViewModel;
        this.optionsViewModel = optionsViewModel;
    }

    public async UniTask StartAsync(CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.StartAsync");
        
        mainViewModel.AddView(addedTransition: true);

        await audioPlayer.InitializeAsync(ct);
        await audioPreloader.PreloadAsync(ct);
        var saves = await findSaveDataUseCase.FindAllAsync(ct);
        
        await mainViewModel.InitializeAsync(core.CurrentViewType, ct);
        await continueViewModel.InitializeAsync(core.CurrentViewType, saves, ct);
        await optionsViewModel.InitializeAsync(core.CurrentViewType, ct);
        
        await UniTask.WaitForSeconds(1.0f, cancellationToken: ct);

        mainViewModel.OpenContinueAsync = OpenContinueAsync;
        mainViewModel.OpenOptionsAsync = OpenOptionsAsync;
        continueViewModel.CloseContinueAsync = CloseContinueAsync;
        optionsViewModel.CloseOptionsAsync = CloseOptionsAsync;
        
        await UniTask.WhenAll(
            mainViewModel.OpenWithoutAddAsync(SceneTransitionState.Next, ct),
            audioPlayer.PlayBgmAsync(audioTable.TitleBgmReference, ct));
    }

    async UniTask OpenContinueAsync(SceneTransitionState state, CancellationToken ct)
    {
        core.CurrentViewType.Value = TitleViewType.Continue;
        await continueViewModel.OpenAsync(state, ct);
    }

    async UniTask OpenOptionsAsync(SceneTransitionState state, CancellationToken ct)
    {
        core.CurrentViewType.Value = TitleViewType.Options;
        await optionsViewModel.OpenAsync(state, ct);
    }

    async UniTask CloseContinueAsync(SceneTransitionState state, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.CloseContinueAsync");
        
        core.CurrentViewType.Value = TitleViewType.Main;
        await continueViewModel.CloseAsync(state, ct);
        mainViewModel.PreOpen();
    }
    
    async UniTask CloseOptionsAsync(SceneTransitionState state, CancellationToken ct)
    {
        core.CurrentViewType.Value = TitleViewType.Main;
        await optionsViewModel.CloseAsync(state, ct);
        mainViewModel.PreOpen();
    }
}