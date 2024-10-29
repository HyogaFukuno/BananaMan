using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace BananaMan.UIElements;

[UxmlElement]
public partial class SaveDataUIElement : VisualElement
{
    readonly VisualElement background = new() { name = "background" };
    readonly VisualElement savedScreenshotElement = new() { name = "saved-screenshot" };
    readonly ZenKakuSmallTextElement savedAtTextElement = new() { name = "saved-at-text" };
    readonly ZenKakuNormalTextElement savedLocationTextElement = new() { name = "saved-location-text" };
    readonly ZenKakuSmallTextElement autoSaveTextElement = new()
    {
        name = "auto-save-text",
        text = @"<style=""bold""><cspace=""2px"">Autosave"
    };
    
    public int Index { get; set; }

    public Sprite? SavedScreenshot
    {
        set => savedScreenshotElement.style.backgroundImage = Background.FromSprite(value);
    }
    
    public DateTimeOffset? SavedAt
    {
        set => savedAtTextElement.text = @$"<style=""bold"">{value:yyyy/MM/dd HH:mm}";
    }
    
    public string? SavedLocation
    {
        set => savedLocationTextElement.text = @$"<style=""bold"">{value}";
    }

    public bool? IsAutoSave
    {
        set => autoSaveTextElement.style.display = value == true
            ? DisplayStyle.Flex
            : DisplayStyle.None;
    }

    public SaveDataUIElement()
    {
        Add(background);
        background.Add(savedScreenshotElement);
        
        var textArea = new VisualElement { name = "text-area" };
        background.Add(textArea);
        
        textArea.Add(savedAtTextElement);
        textArea.Add(savedLocationTextElement);
        textArea.Add(autoSaveTextElement);
    }
}