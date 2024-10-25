using System;
using System.Text.Json.Serialization;

namespace BananaMan.EnterpriseBusinessRules;

public readonly record struct SaveData(
    [property: JsonPropertyName("id")] SaveDataId Id,
    [property: JsonPropertyName("user")] User User,
    [property: JsonPropertyName("auto_save")] bool IsAutoSave,
    [property: JsonPropertyName("saved_screenshot_bytes")] byte[] SavedScreenshotBytes,
    [property: JsonPropertyName("saved_location_name")] string SavedLocationName,
    [property: JsonPropertyName("saved_at")] DateTimeOffset SavedAt) : IComparable<SaveData>
{
    public int CompareTo(SaveData other) => Id.CompareTo(other.Id);
}







public readonly record struct User(
    [property: JsonPropertyName("id")] UserId Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("current_health")] Health CurrentHealth,
    [property: JsonPropertyName("max_health")] Health MaxHealth,
    [property: JsonPropertyName("current_stamina")] Stamina CurrentStamina,
    [property: JsonPropertyName("max_stamina")] Stamina MaxStamina)
{
    public User(UserId id, string name) : this(id, name, 12, 12, 10.0f, 10.0f)
    {
    }
}