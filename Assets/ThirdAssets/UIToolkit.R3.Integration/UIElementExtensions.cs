using R3;
using UnityEngine.UIElements;

namespace UIToolkit.R3.Integration;

public static class UIElementExtensions
{
    public static Observable<TEventType> RegisterCallbackAsObservable<TEventType>(this VisualElement source, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
        where TEventType : EventBase<TEventType>, new()
    {
        return Observable.FromEvent<EventCallback<TEventType>, TEventType>(
            h => x => h(x),
            h => source.RegisterCallback(h, trickleDown),
            h => source.UnregisterCallback(h, trickleDown));
    }
}