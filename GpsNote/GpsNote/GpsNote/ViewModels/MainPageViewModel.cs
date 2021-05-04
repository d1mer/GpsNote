using Prism.Commands;
using Prism.Navigation;
using GpsNote.Services.Localization;
using GpsNote.Views;

namespace GpsNote.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, ILocalizationService localizationService) : base(navigationService, localizationService)
        {
            
        }


        #region -- Public properties --

        private DelegateCommand signInTapCommand;
        public DelegateCommand SignInTapCommand => signInTapCommand ??= new DelegateCommand(OnLoginTapAsync);

        private DelegateCommand signUpTapCommand;
        public DelegateCommand SignUpTapCommand => signUpTapCommand ?? new DelegateCommand(OnRegistrationTapAsync);

        #endregion


        #region -- Private helpers --

        private async void OnLoginTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(SignInPage));
        }

        private async void OnRegistrationTapAsync()
        {
            await NavigationService.NavigateAsync(nameof(SignUpPage));
        }

        #endregion
    }
}