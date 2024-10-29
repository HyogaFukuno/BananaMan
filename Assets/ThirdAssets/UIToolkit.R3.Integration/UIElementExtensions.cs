using System;
using System.Collections.Generic;
using R3;
using UnityEngine.UIElements;

namespace UIToolkit.R3.Integration;

public static class UIElementExtensions
{
    // VisualElement's Observables.
    public static Observable<TEventType> RegisterCallbackAsObservable<TEventType>(this VisualElement source, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
        where TEventType : EventBase<TEventType>, new()
    {
        return Observable.FromEvent<EventCallback<TEventType>, TEventType>(
            h => x => h(x),
            h => source.RegisterCallback(h, trickleDown),
            h => source.UnregisterCallback(h, trickleDown));
    }
    
    public static Observable<(TEventType evt, TArgType arg)> RegisterCallbackAsObservable<TEventType, TArgType>(this VisualElement source, TArgType arg, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
        where TEventType : EventBase<TEventType>, new()
    {
        return Observable.FromEvent<EventCallback<TEventType, TArgType>, (TEventType, TArgType)>(
            h => (x, y) => h((x, y)),
            h => source.RegisterCallback(h, arg, trickleDown),
            h => source.UnregisterCallback(h, trickleDown));
    }
    
    // ListView's Observables.
    public static Observable<IEnumerable<object>> SelectionChangedAsObservable(this ListView source)
    {
        return Observable.FromEvent<Action<IEnumerable<object>>, IEnumerable<object>>(
            h => h,
            h => source.selectionChanged += h,
            h => source.selectionChanged -= h);
    }
    
    public static Observable<IEnumerable<int>> SelectedIndicesChangedAsObservable(this ListView source)
    {
        return Observable.FromEvent<Action<IEnumerable<int>>, IEnumerable<int>>(
            h => h,
            h => source.selectedIndicesChanged += h,
            h => source.selectedIndicesChanged -= h);
    }
    
    public static Observable<IEnumerable<object>> ItemsChosenAsObservable(this ListView source)
    {
        return Observable.FromEvent<Action<IEnumerable<object>>, IEnumerable<object>>(
            h => h,
            h => source.itemsChosen += h,
            h => source.itemsChosen -= h);
    }
}