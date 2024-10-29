using System;
using System.Threading;
using BananaMan.Common.ViewModels;
using BananaMan.Titles.Core;
using BananaMan.Titles.Views;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using R3;
using UIToolkit.R3.Integration;
using UnityEngine;
using UnityEngine.UIElements;
using ZLogger;

namespace BananaMan.Titles.ViewModels;

public sealed class MainViewModel : ViewModelBase<MainView>
{
    enum ModeSelectType
    {
        Continue,
        NewGame,
        Options
    }

    readonly ILogger<MainViewModel> logger;
    ModeSelectType currentMode;
    VisualElement? beforeFocusedElement;
    DisposableBag bag;

    public Func<SceneTransitionState, CancellationToken, UniTask>? OpenContinueAsync { get; set; }
    public Func<SceneTransitionState, CancellationToken, UniTask>? OpenOptionsAsync { get; set; }
    
    public MainViewModel(ILogger<MainViewModel> logger,
                          MainView view,
                          UIDocument rootDocument)
        : base(view, rootDocument, new FadeViewTransition(rootDocument))
    {
        this.logger = logger;
    }

    public async UniTask InitializeAsync(ReadOnlyReactiveProperty<TitleViewType> currentType, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.InitializeAsync");
        
        currentMode = ModeSelectType.Continue;
        
        view.OwnView.RegisterCallbackAsObservable<NavigationMoveEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Main)
            .Subscribe(OnNavigationMove)
            .AddTo(ref bag);
        
        view.OwnView.RegisterCallbackAsObservable<NavigationSubmitEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Main)
            .SubscribeAwait(async (e, ct2) => await OnNavigationSubmitAsync(e, ct2))
            .AddTo(ref bag);
        
        view.OwnView.RegisterCallbackAsObservable<MouseDownEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Main)
            .Subscribe(OnMouseDown)
            .AddTo(ref bag);
        
        view.AppInfoVersionTextElement.text = $"Ver.{Application.version}";
        view.AppInfoCompanyTextElement.text = $"Author {Application.companyName}";
        
        await UniTask.Yield(ct);
    }

    void OnNavigationMove(NavigationMoveEvent e)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnNavigationMove");

        const int min = 0;
        const int max = (int)ModeSelectType.Options;
        var value = (int)currentMode;

        switch (e.direction)
        {
            case NavigationMoveEvent.Direction.Up:
            {
                currentMode = (ModeSelectType)Math.Max(value - 1, min);
                if (value <= min)
                {
                    view.OwnView.focusController.IgnoreEvent(e);
                }
                break;
            }
            case NavigationMoveEvent.Direction.Down:
            {
                currentMode = (ModeSelectType)Math.Min(value + 1, max);
                if (value >= max)
                {
                    view.OwnView.focusController.IgnoreEvent(e);
                }
                break;
            }
        }
    }

    async UniTask OnNavigationSubmitAsync(NavigationSubmitEvent _, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnNavigationSubmitAsync");
        
        switch (currentMode)
        {
            case ModeSelectType.Continue:
            {
                logger.ZLogTrace($"You selected Continue");
                beforeFocusedElement = view.ContinueTextElement;
                await (OpenContinueAsync?.Invoke(SceneTransitionState.Next, ct) ?? UniTask.CompletedTask);
                break;
            }
            case ModeSelectType.NewGame:
            {
                logger.ZLogTrace($"You selected NewGame");
                beforeFocusedElement = view.NewGameTextElement;
                break;
            }
            case ModeSelectType.Options:
            {
                logger.ZLogTrace($"You selected Options");
                beforeFocusedElement = view.OptionsTextElement;
                await (OpenOptionsAsync?.Invoke(SceneTransitionState.Next, ct) ?? UniTask.CompletedTask);
                break;
            }
        }
    }

    void OnMouseDown(MouseDownEvent e)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnMouseDown");
        
        view.OwnView.focusController.IgnoreEvent(e);

        var element = currentMode switch
        {
            ModeSelectType.Continue => view.ContinueTextElement,
            ModeSelectType.NewGame => view.NewGameTextElement,
            ModeSelectType.Options => view.OptionsTextElement,
            _ => null
        };
        
        view.OwnView.schedule.Execute(_ => element?.Focus());
    }
    
    public override void PreOpen()
    {
        (beforeFocusedElement ?? view.ContinueTextElement).Focus();
    }

    protected override void OnDispose()
    {
        bag.Dispose();
    }
}