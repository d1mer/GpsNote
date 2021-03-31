using GpsNote.Services.Registration;
using GpsNote.Services.Authorization;
using GpsNote.Services.Repository;
using GpsNote.ViewModels;
using GpsNote.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using GpsNote.Services.Settings;
using System.Collections;
using System.Collections.Generic;
using GpsNote.Themes;

namespace GpsNote
{
    public partial class App
    {
        private ISettings _settings;

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }


        protected override async void OnInitialized()
        {
            InitializeComponent();

            _settings = Container.Resolve<SettingsService>();

            await NavigationService.NavigateAsync("NavigationPage/MapPage");
            //if (_settings.LoggedUser == -1)
            //   await NavigationService.NavigateAsync("NavigationPage/SignInPage");
            //else
            //    await NavigationService.NavigateAsync(nameof(MainTabbedPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();


            #region service registry

            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<IRegistration>(Container.Resolve<RegistrationService>());
            containerRegistry.RegisterInstance<IAuthorization>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<ISettings>(Container.Resolve<SettingsService>());

            #endregion


            #region navigation registry

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<MapPage, MapViewModel>();
            containerRegistry.RegisterForNavigation<NotesPage, NotesViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();

            #endregion
        }


        #region -- Private helpers --

        private void ResourceLoader()
        {
            ICollection<ResourceDictionary> mergedDictionaries =                   Application.Current.Resources.MergedDictionaries;

            switch (_settings.DarkTheme)
            {
                case false:
                    mergedDictionaries.Add(new LightTheme());
                    break;
                case true:
                    mergedDictionaries.Add(new DarkTheme());
                    break;
            }
        }

        #endregion
    }
}
