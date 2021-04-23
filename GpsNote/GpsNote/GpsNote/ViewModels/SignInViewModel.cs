using System;
using System.ComponentModel;
using Xamarin.Auth;
using Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using GpsNote.Views;
using GpsNote.Services.Authorization;
using GpsNote.Enums;
using GpsNote.Services.Authentication;
using GpsNote.Services.GoogleAuthentication;
using GpsNote.Services.Localization;

namespace GpsNote.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;
        private IAuthorizationService _authorizationService;
        private IAuthenticationService _authenticationService;
        private IGoogleAuthenticationService _googleAuthenticationService;


        public SignInViewModel(INavigationService navigationService,
                               ILocalizationService localizationService,
                               IPageDialogService dialogService, 
                               IAuthorizationService authorizationService, 
                               IAuthenticationService authenticationService,
                               IGoogleAuthenticationService googleAuthenticationService) : base(navigationService, localizationService)
        {
            _dialogService = dialogService;
            _authorizationService = authorizationService;
            _authenticationService = authenticationService;
            _googleAuthenticationService = googleAuthenticationService;

            Title = Resource["LoginText"];
        }


        #region -- Public properties -- 

        private string _email = "";
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _isSignInButtonEnabled;
        public bool IsSignInButtonEnabled
        {
            get => _isSignInButtonEnabled;
            set => SetProperty(ref _isSignInButtonEnabled, value);
        }


        private string errorTextEmail;
        public string ErrorTextEmail 
        {
            get => errorTextEmail;
            set => SetProperty(ref errorTextEmail, value);
        }

        private string errorTextPassword;
        public string ErrorTextPassword 
        {
            get => errorTextPassword;
            set => SetProperty(ref errorTextPassword, value);
        }

        private DelegateCommand onSignUpTapCommand;
        public DelegateCommand OnSignUpTapCommand => onSignUpTapCommand ?? new DelegateCommand(OnNavigationToSignUpAsync);

        private DelegateCommand onSignInButtonTapCommand;
        public DelegateCommand OnSignInButtonTapCommand => onSignInButtonTapCommand ?? new DelegateCommand(OnSignInUserAsync, CanExecute);

        private DelegateCommand onSignInGoogleButtonTapCommand;
        public DelegateCommand OnSignInGoogleButtonTapCommand => onSignInGoogleButtonTapCommand ?? new DelegateCommand(OnSignInGoogleUser);

        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);


        #endregion


        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Password) || args.PropertyName == nameof(Email))
            {
                if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
                    IsSignInButtonEnabled = true;
                else
                    IsSignInButtonEnabled = false;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.TryGetValue<string>(Constants.NEW_USER_EMAIL, out string email))
            {
                Email = email;
            }
        }

        #endregion


        #region Private helpers

        private bool CanExecute()
        {
            return IsSignInButtonEnabled;
        }

        private async void OnNavigationToSignUpAsync()
        {
            await NavigationService.NavigateAsync(nameof(SignUpPage));
        }

        private async void OnSignInUserAsync()
        {
            CodeUserAuthresult result = await _authenticationService.SignInAsync(Email, Password);

            switch (result)
            {
                case CodeUserAuthresult.InvalidEmail:
                    ErrorTextEmail = "Invalid email.Try again";
                    break;
                case CodeUserAuthresult.InvalidPassword:
                    ErrorTextPassword = "Invalid password. Password must be from 8 to 16 characters";
                    break;
                case CodeUserAuthresult.EmailNotFound:
                    ErrorTextEmail = "This email wasn't found";
                    break;
                case CodeUserAuthresult.WrongPassword:
                    ErrorTextPassword = "Wrong password";
                    break;
                case CodeUserAuthresult.Passed:
                    await NavigationService.NavigateAsync($"/{nameof(MainTabbedPage)}");
                    ErrorTextEmail = string.Empty;
                    ErrorTextPassword = string.Empty;
                    break;
                default:
                    ErrorTextEmail = "Error";
                    ErrorTextPassword = "Error";
                    break;
            }
        }

        private void OnSignInGoogleUser()
        {
            _googleAuthenticationService.SignInWithGoogle();
        }

        private void OnBackPressed()
        {
            NavigationService.NavigateAsync($"/{nameof(MainPage)}");
        }

        #endregion
    }
}