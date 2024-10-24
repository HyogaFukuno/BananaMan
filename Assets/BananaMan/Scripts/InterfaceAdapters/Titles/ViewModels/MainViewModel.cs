using System.Threading;
using BananaMan.Common.ViewModels;
using BananaMan.Titles.Views;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnityEngine.UIElements;

namespace BananaMan.Titles.ViewModels;

public sealed class MainViewModel : ViewModelBase<MainView>
{
    readonly ILogger<MainViewModel> logger;
    
    public MainViewModel(ILogger<MainViewModel> logger,
                          MainView view,
                          UIDocument rootDocument)
        : base(view, rootDocument, new FadeViewTransition(rootDocument))
    {
        this.logger = logger;
    }

    public async UniTask InitializeAsync(CancellationToken ct)
    {
        await UniTask.Yield(ct);
    }

    protected override void PreOpen()
    {
    }

    protected override void OnDispose()
    {
    }
}