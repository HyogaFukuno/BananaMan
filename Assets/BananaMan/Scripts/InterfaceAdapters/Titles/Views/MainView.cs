using BananaMan.Common.Views;
using UnityEngine.UIElements;

namespace BananaMan.Titles.Views;

public sealed class MainView : ViewBase
{
    public TextElement ContinueTextElement { get; }
    public TextElement NewGameTextElement { get; }
    public TextElement OptionsTextElement { get; }
    public TextElement AppInfoVersionTextElement { get; }
    public TextElement AppInfoCompanyTextElement { get; }

    public MainView(VisualTreeAsset asset) : base(asset)
    {
        ContinueTextElement = OwnView.Q<TextElement>("continue-text");
        NewGameTextElement = OwnView.Q<TextElement>("new-game-text");
        OptionsTextElement = OwnView.Q<TextElement>("options-text");
        AppInfoVersionTextElement = OwnView.Q<TextElement>("app-info-version-text");
        AppInfoCompanyTextElement = OwnView.Q<TextElement>("app-info-company-text");
    }
}