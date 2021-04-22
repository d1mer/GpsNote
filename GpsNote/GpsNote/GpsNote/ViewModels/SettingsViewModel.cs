using System.ComponentModel;
using Xamarin.Forms;
using Prism.Commands;
using Prism.Navigation;
using GpsNote.Services.Authorization;
using GpsNote.Views;
using GpsNote.Services.Theme;
using GpsNote.Services.Localization;

namespace GpsNote.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private IAuthorizationService _authorizationService;
        private IThemeService _themeService;


        public SettingsViewModel(INavigationService navigationService,
                                 ILocalizationService localizationService,
                                 IAuthorizationService authorizationService, 
                                 IThemeService themeService) : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
            _themeService = themeService;
        }


        #region -- Publics --

        private bool isToogled;
        public bool IsToogled
        {
            get => isToogled;
            set => SetProperty(ref isToogled, value);
        }

        private DelegateCommand logOutCommand;
        public DelegateCommand LogOutCommand => logOutCommand ?? new DelegateCommand(OnLogoutAsync);

        #endregion


        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsToogled))
            {
                OnChangeTheme();
            }
        }

        #endregion


        #region -- Private helpers --

        private async void OnLogoutAsync()
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"/{nameof(MainPage)}");
        }

        private void OnChangeTheme()
        {
            switch (IsToogled)
            {
                case true:
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                    _themeService.Theme = (int)OSAppTheme.Dark;
                    break;
                case false:
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                    _themeService.Theme = (int)OSAppTheme.Light;
                    break;
            }
        }

        #endregion
    }
}