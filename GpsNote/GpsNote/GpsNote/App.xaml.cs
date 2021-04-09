using GpsNote.Services.RegistrationService;
using GpsNote.Services.RepositoryService;
using GpsNote.ViewModels;
using GpsNote.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using GpsNote.Services.SettingsService;
using System.Collections.Generic;
using GpsNote.Themes;
using GpsNote.Services.AuthorizeService;
using GpsNote.Services.UserService;
using GpsNote.Services.PinService;

namespace GpsNote
{
    public partial class App
    {
        private ISettingsService _settings;

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }


        protected override async void OnInitialized()
        {
            InitializeComponent();

            _settings = Container.Resolve<SettingsService>();

            
            if (_settings.IdCurrentUser == -1)
                await NavigationService.NavigateAsync("NavigationPage/SignInPage");
            else
                await NavigationService.NavigateAsync(nameof(MainTabbedPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();


            #region service registry

            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsService>(Container.Resolve<SettingsService>());
            containerRegistry.RegisterInstance<IUserService>(Container.Resolve<UserService>());
            containerRegistry.RegisterInstance<IRegistrationService>(Container.Resolve<RegistrationService>());
            containerRegistry.RegisterInstance<IAuthorizeService>(Container.Resolve<AuthorizeService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());

            #endregion


            #region navigation registry

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<MapPage, MapViewModel>();
            containerRegistry.RegisterForNavigation<NotesPage, NotesViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinPage, AddEditPinViewModel>();

            #endregion
        }


        #region -- Private helpers --

        private void ResourceLoader()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

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
