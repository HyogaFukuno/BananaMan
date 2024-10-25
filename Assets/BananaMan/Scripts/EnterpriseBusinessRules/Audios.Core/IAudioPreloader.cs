using System.Threading;
using Cysharp.Threading.Tasks;

namespace BananaMan.Audios.Core;

public interface IAudioPreloader
{
    UniTask PreloadAsync(CancellationToken ct);
}