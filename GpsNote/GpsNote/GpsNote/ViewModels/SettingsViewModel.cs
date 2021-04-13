using System;
using Xamarin.Forms;
using Prism.Commands;
using Prism.Navigation;
using GpsNote.Services.Authorization;
using GpsNote.Views;


namespace GpsNote.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region -- Private --

        IAuthorizationService _authorizationService;

        #endregion


        public SettingsViewModel(INavigationService navigationService, IAuthorizationService authorizationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
        }


        #region -- Publics --

        private DelegateCommand logOutCommand;
        public DelegateCommand LogOutCommand => logOutCommand ?? (new DelegateCommand(OnLogoutAsync));

        #endregion


        #region -- Private helpers --

        private async void OnLogoutAsync()
        {
            _authorizationService.LogOut();
            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{ nameof(SignInPage)} ");
        }

        #endregion
    }
}