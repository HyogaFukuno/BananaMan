using BananaMan.Common.Views;
using BananaMan.UIElements;
using UnityEngine.UIElements;

namespace BananaMan.Titles.Views;

public sealed class OptionsView : ViewBase
{
    public OptionsDropdownUIElement GraphicsSettingsOptions { get; }
    public OptionsButtonUIElement SoundEnabledOptions { get; }
    public OptionsSliderUIElement BgmVolumeOptions { get; }
    public OptionsSliderUIElement SeVolumeOptions { get; }

    public OptionsView(VisualTreeAsset asset) : base(asset)
    {
        GraphicsSettingsOptions = OwnView.Q<OptionsDropdownUIElement>("quality-settings-options");
        SoundEnabledOptions = OwnView.Q<OptionsButtonUIElement>("sound-enabled-options");
        BgmVolumeOptions = OwnView.Q<OptionsSliderUIElement>("bgm-volume-options");
        SeVolumeOptions = OwnView.Q<OptionsSliderUIElement>("se-volume-options");
    }
}