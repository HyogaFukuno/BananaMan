using UnityEngine.UIElements;

namespace BananaMan.Common.Views;

public abstract class ViewBase
{
    public VisualElement OwnView { get; }

    protected ViewBase(VisualTreeAsset asset)
    {
        OwnView = asset.Instantiate();
    }
}