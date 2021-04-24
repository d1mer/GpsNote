using System.ComponentModel;
using Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using GpsNote.Views;
using GpsNote.Services.Authorization;
using GpsNote.Enums;
using GpsNote.Services.Authentication;
using GpsNote.Services.GoogleAuthentication;
using GpsNote.Services.Localization;
using GpsNote.Helpers;

namespace GpsNote.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly IPageDialogService _dialogService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;


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

            ImageSource = "ic_eye_off.png";
            ShowPassword = false;
            Password = string.Empty;
            Email = string.Empty;
        }


        #region -- Public properties -- 

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
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

        private bool showPassword;
        public bool ShowPassword
        {
            get => showPassword;
            set => SetProperty(ref showPassword, value);
        }

        private string imageSource;
        public string ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        private bool tapPasswordImage;
        public bool TapPasswordImage
        {
            get => tapPasswordImage;
            set => SetProperty(ref tapPasswordImage, value);
        }

        private bool tapEmailImage;
        public bool TapEmailImage
        {
            get => tapEmailImage;
            set => SetProperty(ref tapEmailImage, value);
        }


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
                {
                    IsSignInButtonEnabled = true;
                }
                else if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    IsSignInButtonEnabled = false;
                }            
            }
            else if(args.PropertyName == nameof(TapPasswordImage))
            {
                ImageSource = TapPasswordImage ? "ic_eye.png" : "ic_eye_off.png";
                ShowPassword = TapPasswordImage;
            }
            else if(args.PropertyName == nameof(TapEmailImage))
            {
                Email = string.Empty;
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

        private async void OnSignInUserAsync()
        {
            Email = Email.Trim();
            Password = Password.Trim();

            bool verifyEmail = Validator.Validate(Email, VerifyEntity.Email);
            bool verifyPassword = Validator.Validate(Password, VerifyEntity.Password);

            if(verifyEmail && verifyPassword)
            {
                var (resultEmail, resultPassword) =
                    await _authenticationService.OnSignInAsync(Email, Password);

                switch (resultEmail)
                {
                    case CodeUserAuthresult.InvalidEmail:
                        ErrorTextEmail = "Invalid email.Try again";
                        break;
                    case CodeUserAuthresult.EmailNotFound:
                        ErrorTextEmail = "This email wasn't found";
                        break;
                    case CodeUserAuthresult.Passed:
                        break;
                    default:
                        ErrorTextEmail = "Error";
                        break;
                }

                switch (resultPassword)
                {
                    case CodeUserAuthresult.InvalidPassword:
                        ErrorTextPassword = "Invalid password. Password must be from 8 to 16 characters";
                        break;
                    case CodeUserAuthresult.WrongPassword:
                        ErrorTextPassword = "Wrong password";
                        break;
                    case CodeUserAuthresult.Passed:
                        break;
                    default:
                        ErrorTextPassword = "Error";
                        break;
                }

                if (resultEmail == CodeUserAuthresult.Passed && resultPassword == CodeUserAuthresult.Passed)
                {
                    await NavigationService.NavigateAsync($"/{nameof(MainTabbedPage)}");
                    ErrorTextEmail = string.Empty;
                    ErrorTextPassword = string.Empty;
                }
            }
            else
            {
                ErrorTextEmail = verifyEmail ? 
                                 string.Empty :
                                 "Invalid email.Try again";

                ErrorTextPassword = verifyPassword ? 
                                    string.Empty : 
                                    "Invalid password. Password must be from 8 to 16 characters";
                return;
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