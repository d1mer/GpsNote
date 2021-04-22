using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using GpsNote.Services.Authentication;
using GpsNote.Services.RepositoryService;
using GpsNote.ViewModels;
using GpsNote.Views;
using GpsNote.Services.SettingsService;
using GpsNote.Services.Authorization;
using GpsNote.Services.PinService;
using GpsNote.Services.MapCameraSettingsService;
using GpsNote.Services.Theme;
using GpsNote.Services.Permissions;
using GpsNote.Services.GoogleAuthentication;
using GpsNote.Services.TimeZone;
using GpsNote.Views.Clock;
using GpsNote.ViewModels.Clock;
using GpsNote.Services.Color;

namespace GpsNote
{
    public partial class App
    {
        private IAuthorizationService _authorizationService;
        private IAuthorizationService AuthorizationService =>
            _authorizationService ?? (_authorizationService = Container.Resolve<IAuthorizationService>());

        private IThemeService _themeService;
        public IThemeService ThemeService =>
            _themeService ?? (_themeService = Container.Resolve<IThemeService>());

        private IPermissionsService _permissionsService;
        public IPermissionsService PermissionsService => _permissionsService ?? (_permissionsService = Container.Resolve<IPermissionsService>());

        private ISettingsManager _settingsManager;
        public ISettingsManager SettingsManager => _settingsManager ?? (_settingsManager = Container.Resolve<ISettingsManager>());

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }


        protected override async void OnInitialized()
        {
            InitializeComponent();

            Application.Current.UserAppTheme = (OSAppTheme)ThemeService.Theme;

            if (!AuthorizationService.IsAuthorized())
            {
                await NavigationService.NavigateAsync(nameof(MainPage));
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(MainTabbedPage));
            }

            PermissionStatus locationPermission = await PermissionsService.CheckStatusAsync<LocationPermission>();

            if (locationPermission != PermissionStatus.Granted)
            {
                if (await PermissionsService.ShowRequestPermission<LocationPermission>())
                {
                    SettingsManager.LocationPermission = true;
                }
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>()); 
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<IMapCameraSettingsService>(Container.Resolve<MapCameraSettingsService>());
            containerRegistry.RegisterInstance<IThemeService>(Container.Resolve<ThemeService>());
            containerRegistry.RegisterInstance<IPermissionsService>(Container.Resolve<PermissionsService>());
            containerRegistry.RegisterInstance<IGoogleAuthenticationService>(Container.Resolve<GoogleAuthenticationService>());
            containerRegistry.RegisterInstance<ITimeZoneService>(Container.Resolve<TimeZoneService>());
            containerRegistry.RegisterInstance<IColorService>(Container.Resolve<ColorService>());

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<MapPage, MapViewModel>();
            containerRegistry.RegisterForNavigation<NotesPage, NotesViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinPage, AddEditPinViewModel>();
            containerRegistry.RegisterForNavigation<ClockPopupPage, ClockPopupViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }
    }
}