using BananaMan.Common.Views;
using UnityEngine.UIElements;

namespace BananaMan.Titles.Views;

public sealed class ContinueView : ViewBase
{
    public ListView SaveDataListView { get; }

    public ContinueView(VisualTreeAsset asset) : base(asset)
    {
        SaveDataListView = OwnView.Q<ListView>();
    }
}