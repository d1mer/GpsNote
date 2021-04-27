using GpsNote.Enums;
using GpsNote.Helpers;
using GpsNote.Services.Authentication;
using GpsNote.Services.GoogleAuthentication;
using GpsNote.Services.Localization;
using GpsNote.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GpsNote.ViewModels
{
    public class SignUpViewModel2 : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;
        private readonly IPageDialogService _dialogService;

        public SignUpViewModel2(INavigationService navigationService,
                                ILocalizationService localizationService,
                                IGoogleAuthenticationService googleAuthenticationService,
                                IAuthenticationService authenticationService,
                                IPageDialogService pageDialogService) : base(navigationService, localizationService)
        {
            _authenticationService = authenticationService;
            _googleAuthenticationService = googleAuthenticationService;
            _dialogService = pageDialogService;

            Title = Resource["CreateAccountTitle"];

            ImageSourcePassword = "ic_eye_off.png";
            ImageSourceConfirmPassword = "ic_eye_off.png";
            ShowPassword = false;
            ShowConfirmPassword = false;
        }

        #region -- Public properties --

        private string imageSourcePassword;
        public string ImageSourcePassword
        {
            get => imageSourcePassword;
            set => SetProperty(ref imageSourcePassword, value);
        }

        private string imageSourceConfirmPassword;
        public string ImageSourceConfirmPassword
        {
            get => imageSourceConfirmPassword;
            set => SetProperty(ref imageSourceConfirmPassword, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword; 
            set => SetProperty(ref confirmPassword, value);
        }

        private string errorTextPassword;
        public string ErrorTextPassword
        {
            get => errorTextPassword;
            set => SetProperty(ref errorTextPassword, value);
        }

        private string errorTextConfirmPassword;
        public string ErrorTextConfirmPassword
        {
            get => errorTextConfirmPassword;
            set => SetProperty(ref errorTextConfirmPassword, value);
        }

        private bool showPassword;
        public bool ShowPassword
        {
            get => showPassword;
            set => SetProperty(ref showPassword, value);
        }

        private bool showConfirmPassword;
        public bool ShowConfirmPassword
        {
            get => showConfirmPassword;
            set => SetProperty(ref showConfirmPassword, value);
        }

        private bool tapPasswordImage;
        public bool TapPasswordImage
        {
            get => tapPasswordImage;
            set => SetProperty(ref tapPasswordImage, value);
        }

        private bool tapConfirmPasswordImage;
        public bool TapConfirmPasswordImage
        {
            get => tapConfirmPasswordImage;
            set => SetProperty(ref tapConfirmPasswordImage, value);
        }

        private bool isSignUpButtonEnabled;
        public bool IsSignUpButtonEnabled
        {
            get => isSignUpButtonEnabled;
            set => SetProperty(ref isSignUpButtonEnabled, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private DelegateCommand signUpButtonTapCommand;
        public DelegateCommand SignUpButtonTapCommand => signUpButtonTapCommand ?? new DelegateCommand(OnSignUpTapAsync, CanExecute);

        private DelegateCommand signInGoogleButtonTapCommand;
        public DelegateCommand SignInGoogleButtonTapCommand => signInGoogleButtonTapCommand ?? new DelegateCommand(OnSignInGoogleAsync);

        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);

        #endregion


        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.PropertyName == nameof(Password) || args.PropertyName == nameof(ConfirmPassword))
            {
                if(!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    IsSignUpButtonEnabled = true;
                }
                else
                {
                    IsSignUpButtonEnabled = false;
                    ErrorTextPassword = string.Empty;
                    ErrorTextConfirmPassword = string.Empty;
                }
            }
            else if(args.PropertyName == nameof(TapPasswordImage))
            {
                ImageSourcePassword = TapPasswordImage ? "ic_eye.png" : "ic_eye_off.png";
                ShowPassword = TapPasswordImage;
            }
            else if (args.PropertyName == nameof(TapConfirmPasswordImage))
            {
                ImageSourceConfirmPassword = TapConfirmPasswordImage ? "ic_eye.png" : "ic_eye_off.png";
                ShowConfirmPassword = TapConfirmPasswordImage;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.TryGetValue<(string, string)>(Constants.DATA_USER, out (string, string) userData))
            {
                Email = userData.Item1;
                Name = userData.Item2;
            }
        }

        #endregion


        #region -- Private helpers --

        private bool CanExecute()
        {
            return IsSignUpButtonEnabled;
        }

        private void OnBackPressed()
        {
            NavigationService.GoBackAsync();
        }

        private void OnSignInGoogleAsync()
        {
            _googleAuthenticationService.SignInWithGoogle();
        }

        private async void OnSignUpTapAsync()
        {
            Password = Password.Trim();
            ConfirmPassword = ConfirmPassword.Trim();

            if(Password == ConfirmPassword)
            {
                bool verifyPassword = Validator.Validate(Password, VerifyEntity.Password);

                if (verifyPassword)
                {
                    ErrorTextPassword = string.Empty;
                    ErrorTextConfirmPassword = string.Empty;

                    bool result = await _authenticationService.OnSignUpAsync(Name, Email, Password);

                    if (result)
                    {
                        await _dialogService.DisplayAlertAsync(Resource["SuccessText"],
                                                               Resource["AccountCreatedText"],
                                                               Resource["CancelText"]);

                        NavigationParameters parameter = new NavigationParameters();
                        parameter.Add(Constants.NEW_USER_EMAIL, Email);
                        await NavigationService.NavigateAsync($"{nameof(MainPage)}/{nameof(SignInPage)}", parameter);
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync(Resource["ErrorText"], Resource["ErrorSaveDbText"], Resource["CancelText"]);
                    }
                }
                else
                {
                    ErrorTextPassword = Resource["WrongPasswordError"];
                }
            }
            else
            {
                ErrorTextConfirmPassword = Resource["PasswordMismatchError"];
            }
        }

        #endregion
    }
}