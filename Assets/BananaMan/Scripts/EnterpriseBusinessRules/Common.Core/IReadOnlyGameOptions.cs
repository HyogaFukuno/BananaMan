using FastEnumUtility;
using R3;

namespace BananaMan.Common.Core;

public enum GraphicsSettingsType
{
    [Label("低設定")]
    Low,
            
    [Label("中設定")]
    Medium,
            
    [Label("高設定")]
    High
}

public interface IReadOnlyGameOptions
{
    public Observable<GraphicsSettingsType> GraphicsSettings { get; }
    public Observable<bool> SoundEnabled { get; }
    public Observable<float> BgmVolume { get; }
    public Observable<float> SeVolume { get; }
}