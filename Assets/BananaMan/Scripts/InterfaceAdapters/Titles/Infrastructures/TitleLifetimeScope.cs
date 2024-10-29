using System;
using BananaMan.ApplicationBusinessRules.UseCases;
using BananaMan.Titles.Core;
using BananaMan.Titles.Presenter;
using BananaMan.Titles.ViewModels;
using BananaMan.Titles.Views;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace BananaMan.Titles.Infrastructures
{
    [Serializable]
    public sealed class TitleLifetimeScope : LifetimeScope
    {
        [SerializeField] UIDocument? rootDocument;
        [SerializeField] VisualTreeAsset? mainTreeAsset;
        [SerializeField] VisualTreeAsset? continueTreeAsset;
        [SerializeField] VisualTreeAsset? optionsTreeAsset;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<TitlePresenter>();
            builder.RegisterComponent(rootDocument);

            builder.Register<TitleModel>(Lifetime.Scoped);
            
            builder.Register<MainViewModel>(Lifetime.Scoped);
            builder.Register<MainView>(Lifetime.Scoped).WithParameter(mainTreeAsset);
            
            builder.Register<ContinueViewModel>(Lifetime.Scoped);
            builder.Register<ContinueView>(Lifetime.Scoped).WithParameter(continueTreeAsset);
            
            builder.Register<OptionsViewModel>(Lifetime.Scoped);
            builder.Register<OptionsView>(Lifetime.Scoped).WithParameter(optionsTreeAsset);

            builder.Register<FindSaveDataUseCase>(Lifetime.Scoped);
        }
    }
}