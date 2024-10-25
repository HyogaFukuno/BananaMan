using System.Collections.Generic;
using System.Threading;
using BananaMan.ApplicationBusinessRules.Interfaces;
using BananaMan.EnterpriseBusinessRules;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace BananaMan.ApplicationBusinessRules.UseCases;

public sealed class FindSaveDataUseCase
{
    readonly ILogger<FindSaveDataUseCase> logger;
    readonly IAsyncRepository<SaveData, SaveDataId> repository;

    public FindSaveDataUseCase(ILogger<FindSaveDataUseCase> logger,
                               IAsyncRepository<SaveData, SaveDataId> repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async UniTask<IEnumerable<SaveData>> FindAllAsync(CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.FindAllAsync");

        return await repository.FindAllAsync(ct);
    }

    public async UniTask<SaveData?> FindByIdAsync(SaveDataId id, CancellationToken ct)
    {
        logger.ZLogTrace($"Called {GetType().Name}.FindAsync id: {id}");

        return await repository.FindAsync(id, ct);
    }
}