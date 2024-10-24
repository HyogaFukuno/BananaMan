using System.Threading;
using Cysharp.Threading.Tasks;

namespace BananaMan.Common.ViewModels;

public interface IViewTransition
{
    void AddTransition();
    void RemoveTransition();
    UniTask TransitionInAsync(SceneTransitionState state, CancellationToken ct);
    UniTask TransitionOutAsync(SceneTransitionState state, CancellationToken ct);
}