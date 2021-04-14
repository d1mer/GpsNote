using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using GpsNote.Services.Authentication;
using GpsNote.Services.RepositoryService;
using GpsNote.ViewModels;
using GpsNote.Views;
using GpsNote.Services.SettingsService;
using GpsNote.Services.Authorization;
using GpsNote.Services.PinService;
using GpsNote.Services.MapCameraSettingsService;
using GpsNote.Services.Theme;

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
            containerRegistry.RegisterInstance<IThemeService>(Container.Resolve<ThemeService>());

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
    }
}