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
        private OSAppTheme _appTheme;


        public SettingsViewModel(INavigationService navigationService,
                                 ILocalizationService localizationService,
                                 IAuthorizationService authorizationService, 
                                 IThemeService themeService) : base(navigationService, localizationService)
        {
            _authorizationService = authorizationService;
            _themeService = themeService;
        }


        #region -- Public properties --

        private bool isToogled;
        public bool IsToogled
        {
            get => isToogled;
            set => SetProperty(ref isToogled, value);
        }

        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);

        private DelegateCommand clockColorArrowTap;
        public DelegateCommand ClockColorArrowTap => clockColorArrowTap ?? new DelegateCommand(OnClockSettingsTapAsync);

        private DelegateCommand languageArrowTap;
        public DelegateCommand LanguageArrowTap => languageArrowTap ?? new DelegateCommand(OnLanguageSettingsTapAsync);

        #endregion


        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            _appTheme = (OSAppTheme)_themeService.Theme;

            switch (_appTheme)
            {
                case OSAppTheme.Light:
                    IsToogled = false;
                    break;
                case OSAppTheme.Dark:
                    IsToogled = true;
                    break;
            }
        }

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
            await NavigationService.NavigateAsync(nameof(MainPage));
        }

        private void OnBackPressed()
        {
            NavigationService.GoBackAsync();
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

        private async void OnClockSettingsTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(SettingsClock));
        }

        private async void OnLanguageSettingsTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(SettingsLanguage));
        }

        #endregion
    }
}