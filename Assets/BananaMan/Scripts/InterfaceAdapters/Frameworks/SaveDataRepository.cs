using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BananaMan.ApplicationBusinessRules.Interfaces;
using BananaMan.EnterpriseBusinessRules;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace BananaMan.Frameworks;

public sealed class SaveDataRepository : IAsyncRepository<SaveData, SaveDataId>
{
    readonly ILogger<SaveDataRepository> logger;
    readonly IAsyncDataStore<SaveData[]> dataStore;
    SaveData[]? data;

    public SaveDataRepository(ILogger<SaveDataRepository> logger,
                              IAsyncDataStore<SaveData[]> dataStore)
    {
        this.logger = logger;
        this.dataStore = dataStore;
    }

    public async UniTask<IEnumerable<SaveData>> FindAllAsync(CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.FindAllAsync");
        
        return (data ??= await dataStore.LoadAsync(ct))?
            .AsEnumerable() ?? Enumerable.Empty<SaveData>();
    }

    public async UniTask<SaveData?> FindAsync(SaveDataId key, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.FindAsync Key: {key}");
        return (data ??= await dataStore.LoadAsync(ct))?.FirstOrDefault(x => x.Id == key);
    }

    public async UniTask StoreAsync(SaveData? value, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.StoreAsync value: {value}");

        if (value is null)
        {
            return;
        }

        data ??= await dataStore.LoadAsync(ct);
        
        var currentLength = data?.Length ?? throw new NullReferenceException();
        Array.Resize(ref data, currentLength + 1);
        data[currentLength] = value.Value;
        
        await dataStore.StoreAsync(data, ct);
    }
}