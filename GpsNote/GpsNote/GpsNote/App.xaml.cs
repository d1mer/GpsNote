using GpsNote.Services.Authentication;
using GpsNote.Services.RepositoryService;
using GpsNote.ViewModels;
using GpsNote.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using GpsNote.Services.SettingsService;
using GpsNote.Services.Authorization;
using GpsNote.Services.PinService;
using Unity;
using GpsNote.Services.MapCameraSettingsService;

namespace GpsNote
{
    public partial class App
    {
        private IAuthorizationService _authorizationService;
        private IAuthorizationService AuthorizationService =>
            _authorizationService ?? (_authorizationService = Container.Resolve<IAuthorizationService>());

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }


        protected override async void OnInitialized()
        {
            InitializeComponent();

            if (!AuthorizationService.IsAuthorized())
            {
                await NavigationService.NavigateAsync("NavigationPage/SignInPage");
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(MainTabbedPage));
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>()); 
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<ICameraSettingsService>(Container.Resolve<CameraSettingsService>());

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<MapPage, MapViewModel>();
            containerRegistry.RegisterForNavigation<NotesPage, NotesViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinPage, AddEditPinViewModel>();
        }


        #region -- Private helpers --

        private void ResourceLoader()
        {
            //ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            //switch (_settings.DarkTheme)
            //{
            //    case false:
            //        mergedDictionaries.Add(new LightTheme());
            //        break;
            //    case true:
            //        mergedDictionaries.Add(new DarkTheme());
            //        break;
            //}
        }

        #endregion
    }
}
