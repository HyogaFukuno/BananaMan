using System.Threading;
using Cysharp.Threading.Tasks;

namespace BananaMan.Frameworks;

public interface IDataStore<T>
{
    T? Load();
    void Store(T? data);
}

public interface IAsyncDataStore<T> 
{
    UniTask<T?> LoadAsync(CancellationToken token);
    UniTask StoreAsync(T? data, CancellationToken token);
}