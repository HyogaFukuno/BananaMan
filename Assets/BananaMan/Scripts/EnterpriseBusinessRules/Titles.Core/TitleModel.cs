using R3;

namespace BananaMan.Titles.Core;

public enum TitleViewType
{
    Main,
    Continue,
    Options
}

public sealed class TitleModel
{
    public ReactiveProperty<TitleViewType> CurrentViewType { get; } = new();
}