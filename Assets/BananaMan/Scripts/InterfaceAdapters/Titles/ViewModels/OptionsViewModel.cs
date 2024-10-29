using System;
using System.Threading;
using BananaMan.Common.ViewModels;
using BananaMan.Titles.Core;
using BananaMan.Titles.Views;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using R3;
using UIToolkit.R3.Integration;
using UnityEngine.UIElements;
using ZLogger;

namespace BananaMan.Titles.ViewModels;

public sealed class OptionsViewModel : ViewModelBase<OptionsView>
{
    enum OptionsType
    {
        GraphicsSettings,
        SoundEnabled,
        BgmVolume,
        SeVolume
    }

    readonly ILogger<OptionsViewModel> logger;
    OptionsType currentOptions;
    DisposableBag bag;
    
    public OptionsViewModel(ILogger<OptionsViewModel> logger,
                            OptionsView view,
                            UIDocument rootDocument)
        : base(view, rootDocument, new TransparentViewTransition(view.OwnView))
    {
        this.logger = logger;
    }
    
    public Func<SceneTransitionState, CancellationToken, UniTask>? CloseOptionsAsync { get; set; }

    public async UniTask InitializeAsync(ReadOnlyReactiveProperty<TitleViewType> currentType, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.InitializeAsync");

        currentOptions = OptionsType.GraphicsSettings;

        view.OwnView.RegisterCallbackAsObservable<NavigationMoveEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Options)
            .Subscribe(OnNavigationMove)
            .AddTo(ref bag);
        
        view.OwnView.RegisterCallbackAsObservable<NavigationSubmitEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Options)
            .Subscribe(OnNavigationSubmit)
            .AddTo(ref bag);
        
        view.OwnView.RegisterCallbackAsObservable<NavigationCancelEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Options)
            .SubscribeAwait(async (x, ct2) => await OnNavigationCancelAsync(x, ct2))
            .AddTo(ref bag);
        
        view.OwnView.RegisterCallbackAsObservable<MouseDownEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Options)
            .Subscribe(OnMouseDown)
            .AddTo(ref bag);

        await UniTask.Yield(ct);
    }

    void OnNavigationMove(NavigationMoveEvent e)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnNavigationMove");
        
        const int min = 0;
        const int max = (int)OptionsType.SeVolume;
        var value = (int)currentOptions;

        switch (e.direction)
        {
            case NavigationMoveEvent.Direction.Up:
            {
                currentOptions = (OptionsType)Math.Max(value - 1, min);
                if (value <= min)
                {
                    view.OwnView.focusController.IgnoreEvent(e);
                }
                break;
            }
            case NavigationMoveEvent.Direction.Down:
            {
                currentOptions = (OptionsType)Math.Min(value + 1, max);
                if (value >= max)
                {
                    view.OwnView.focusController.IgnoreEvent(e);
                }
                break;
            }
        }
    }

    void OnNavigationSubmit(NavigationSubmitEvent e)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnNavigationSubmit");

        switch (currentOptions)
        {
            case OptionsType.GraphicsSettings:
            {
                view.GraphicsSettingsOptions.FocusDropdown();
                break;
            }
            case OptionsType.SoundEnabled:
            {
                view.SoundEnabledOptions.FocusButton();
                break;
            }
            case OptionsType.BgmVolume:
            {
                view.BgmVolumeOptions.FocusSlider();
                break;
            }
            case OptionsType.SeVolume:
            {
                view.BgmVolumeOptions.FocusSlider();
                break;
            }
        }
    }
    
    void OnMouseDown(MouseDownEvent e)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnMouseDown");
        
        view.OwnView.focusController.IgnoreEvent(e);

        VisualElement? element = currentOptions switch
        {
            OptionsType.GraphicsSettings => view.GraphicsSettingsOptions,
            OptionsType.SoundEnabled => view.SoundEnabledOptions,
            OptionsType.BgmVolume => view.BgmVolumeOptions,
            OptionsType.SeVolume => view.SeVolumeOptions,
            _ => null
        };
        
        view.OwnView.schedule.Execute(_ => element?.Focus());
    }

    async UniTask OnNavigationCancelAsync(NavigationCancelEvent _, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnNavigationCancelAsync");
        
        await (CloseOptionsAsync?.Invoke(SceneTransitionState.Previous, ct) ?? UniTask.CompletedTask);
    }

    public override void PreOpen()
    {
        view.GraphicsSettingsOptions.Focus();
    }

    protected override void OnDispose()
    {
        bag.Dispose();
    }
}