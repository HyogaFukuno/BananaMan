using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using BananaMan.ApplicationBusinessRules.UseCases;
using BananaMan.EnterpriseBusinessRules;
using BananaMan.Frameworks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.TestTools;
using ZLogger;
using ZLogger.Unity;

namespace BananaMan.Tests;

public sealed class UserUseCaseTester
{
    [UnityTest]
    public IEnumerator TestCreateUserUseCase() => UniTask.ToCoroutine(async () =>
    {
        var cts = new CancellationTokenSource();
        
        var path = Path.Combine(Application.streamingAssetsPath, "master_data", "save_data.json");
        var dataStore = new JsonAsyncDataStore<SaveData[]>(path);
        
        var user = new User(0, "Glacier", 12, 12, 10, 10);
        await dataStore.StoreAsync(new []
        {
            new SaveData
            {
                Id = 0,
                User = user,
                IsAutoSave = false,
                SavedScreenshotBytes = Encoding.GetEncoding("UTF-8").GetBytes("hoge hoge"),
                SavedLocationName = "HogeHoge Mountain",
                SavedAt = DateTimeOffset.Now
            },
            new SaveData
            {
                Id = 1,
                User = user,
                IsAutoSave = true,
                SavedScreenshotBytes = Encoding.GetEncoding("UTF-8").GetBytes("hoge hoge"),
                SavedLocationName = "HogeHoge Highway",
                SavedAt = DateTimeOffset.Now
            },
            new SaveData
            {
                Id = 2,
                User = user,
                IsAutoSave = true,
                SavedScreenshotBytes = Encoding.GetEncoding("UTF-8").GetBytes("hoge hoge"),
                SavedLocationName = "HogeHoge Island",
                SavedAt = DateTimeOffset.Now
            },
            new SaveData
            {
                Id = 3,
                User = user,
                IsAutoSave = false,
                SavedScreenshotBytes = Encoding.GetEncoding("UTF-8").GetBytes("hoge hoge"),
                SavedLocationName = "HogeHoge Island",
                SavedAt = DateTimeOffset.Now
            },
            new SaveData
            {
                Id = 4,
                User = user,
                IsAutoSave = true,
                SavedScreenshotBytes = Encoding.GetEncoding("UTF-8").GetBytes("hoge hoge"),
                SavedLocationName = "HogeHoge Village",
                SavedAt = DateTimeOffset.Now
            },
            new SaveData
            {
                Id = 5,
                User = user,
                IsAutoSave = false,
                SavedScreenshotBytes = Encoding.GetEncoding("UTF-8").GetBytes("hoge hoge"),
                SavedLocationName = "HogeHoge Castle",
                SavedAt = DateTimeOffset.Now
            }
        } , cts.Token);
    });
    
    [UnityTest]
    public IEnumerator TestLoadUserUseCase() => UniTask.ToCoroutine(async () =>
    {
        var factory = LoggerFactory.Create(static logging =>
        {
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddZLoggerUnityDebug();
        });
        
        var logger = factory.CreateLogger<UserUseCaseTester>();

        var cts = new CancellationTokenSource();
        
        var path = Path.Combine(Application.streamingAssetsPath, "master_data", "save_data.json");
        var dataStore = new JsonAsyncDataStore<SaveData[]>(path);
        
        var saves = await dataStore.LoadAsync(cts.Token);
        for (var i = 0; i < saves?.Length; i++)
        {
            logger.ZLogTrace($"SaveData: {saves[i]}");
        }
    });
    
    [UnityTest]
    public IEnumerator TestSaveDataRepository() => UniTask.ToCoroutine(async () =>
    {
        var factory = LoggerFactory.Create(static logging =>
        {
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddZLoggerUnityDebug();
        });
        
        var logger = factory.CreateLogger<UserUseCaseTester>();

        var cts = new CancellationTokenSource();
        
        var path = Path.Combine(Application.streamingAssetsPath, "master_data", "save_data.json");
        var dataStore = new JsonAsyncDataStore<SaveData[]>(path);
        var repository = new SaveDataRepository(factory.CreateLogger<SaveDataRepository>(), dataStore);
        
        foreach (var save in await repository.FindAllAsync(cts.Token))
        {
            logger.ZLogTrace($"SaveData: {save}");
        }
    });
    
    [UnityTest]
    public IEnumerator TestSaveDataUseCase() => UniTask.ToCoroutine(async () =>
    {
        var factory = LoggerFactory.Create(static logging =>
        {
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddZLoggerUnityDebug();
        });
        
        var logger = factory.CreateLogger<UserUseCaseTester>();

        var cts = new CancellationTokenSource();
        
        var path = Path.Combine(Application.streamingAssetsPath, "master_data", "save_data.json");
        var dataStore = new JsonAsyncDataStore<SaveData[]>(path);
        var repository = new SaveDataRepository(factory.CreateLogger<SaveDataRepository>(), dataStore);
        var useCase = new FindSaveDataUseCase(factory.CreateLogger<FindSaveDataUseCase>(), repository);
        
        foreach (var save in await useCase.FindAllAsync(cts.Token))
        {
            logger.ZLogTrace($"SaveData: {save}");
        }
    });
}