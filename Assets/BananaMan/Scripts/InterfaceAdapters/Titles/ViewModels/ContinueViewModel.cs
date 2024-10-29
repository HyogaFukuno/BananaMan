using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BananaMan.Common.ViewModels;
using BananaMan.EnterpriseBusinessRules;
using BananaMan.Titles.Core;
using BananaMan.Titles.Views;
using BananaMan.UIElements;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using R3;
using UIToolkit.R3.Integration;
using UnityEngine;
using UnityEngine.UIElements;
using ZLogger;

namespace BananaMan.Titles.ViewModels;

public sealed class ContinueViewModel : ViewModelBase<ContinueView>
{
    readonly ILogger<ContinueViewModel> logger;
    List<SaveData>? saves;
    int currentSaveDataIndex;
    DisposableBag bag;
    
    public Func<SceneTransitionState, CancellationToken, UniTask>? CloseContinueAsync { get; set; }
    
    public ContinueViewModel(ILogger<ContinueViewModel> logger, 
                             ContinueView view,
                             UIDocument rootDocument) 
        : base(view, rootDocument, new TransparentViewTransition(view.OwnView))
    {
        this.logger = logger;
    }

    public async UniTask InitializeAsync(ReadOnlyReactiveProperty<TitleViewType> currentType,
                                         IEnumerable<SaveData>? saves,
                                         CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.InitializeAsync");
        
        var itemsSource = saves?.OrderByDescending(static x => x.Id).ToList();
        this.saves = itemsSource;
        
        view.SaveDataListView.itemsSource = itemsSource;
        view.SaveDataListView.makeItem = CreateSaveDataUIElement;
        view.SaveDataListView.bindItem = OnBindSaveDataUIElement;
        view.SaveDataListView.fixedItemHeight = 130.0f;
        
        view.OwnView.RegisterCallbackAsObservable<NavigationCancelEvent>(TrickleDown.TrickleDown)
            .SubscribeAwait(async (x, ct2) => await OnNavigationCancelAsync(x, ct2))
            .AddTo(ref bag);
        
        view.OwnView.RegisterCallbackAsObservable<MouseDownEvent>(TrickleDown.TrickleDown)
            .Where(_ => currentType.CurrentValue == TitleViewType.Continue)
            .Subscribe(OnMouseDown)
            .AddTo(ref bag);
        
        view.SaveDataListView.SelectedIndicesChangedAsObservable()
            .Where(_ => currentType.CurrentValue == TitleViewType.Continue)
            .Select(x => x.FirstOrDefault())
            .Subscribe(OnSelectedIndicesChanged)
            .AddTo(ref bag);
        
        view.SaveDataListView.ItemsChosenAsObservable()
            .Where(_ => currentType.CurrentValue == TitleViewType.Continue)
            .SubscribeAwait(async (_, ct2) => await OnItemsChosenAsync(ct2))
            .AddTo(ref bag);
        
        await UniTask.Yield(ct);
    }
    
    static VisualElement CreateSaveDataUIElement() => new SaveDataUIElement
    {
        focusable = true,
        tabIndex = 0
    };

    void OnBindSaveDataUIElement(VisualElement element, int index)
    {
        if (element is not SaveDataUIElement saveDataElement)
        {
            return;
        }

        saveDataElement.name = $"save_data_{index}";
        saveDataElement.SavedAt = saves?[index].SavedAt;
        saveDataElement.SavedLocation = saves?[index].SavedLocationName;
        saveDataElement.IsAutoSave = saves?[index].IsAutoSave;
    }

    async UniTask OnNavigationCancelAsync(NavigationCancelEvent _, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnNavigationCancelAsync");
        
        await (CloseContinueAsync?.Invoke(SceneTransitionState.Previous, ct) ?? UniTask.CompletedTask);
    }
    
    void OnMouseDown(MouseDownEvent e)
    {
        logger.ZLogTrace($"Called {GetType().Name}.OnMouseDown");
        
        view.OwnView.focusController.IgnoreEvent(e);
        view.OwnView.schedule.Execute(_ => view.SaveDataListView.SetSelection(currentSaveDataIndex));
    }

    void OnSelectedIndicesChanged(int index)
    {
        currentSaveDataIndex = index;
    }

    async UniTask OnItemsChosenAsync(CancellationToken ct)
    {
        var selectedSaveData = saves![currentSaveDataIndex];
        logger.ZLogInformation($"You selected {selectedSaveData}");
        await UniTask.Yield(ct);
    }

    public override void PreOpen()
    {
        view.SaveDataListView.Focus();
        view.SaveDataListView.SetSelection(currentSaveDataIndex = 0);
    }

    protected override void OnDispose()
    {
        bag.Dispose();
    }
}