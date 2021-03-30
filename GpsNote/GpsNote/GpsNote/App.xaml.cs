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

namespace GpsNote
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            ISettings settings = Container.Resolve<SettingsService>();
            if(settings.LoggedUser == -1)
               await NavigationService.NavigateAsync("NavigationPage/SignInPage");
            else
                await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage");
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

            #endregion
        }
    }
}
