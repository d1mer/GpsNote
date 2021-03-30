using GpsNote.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Prism.Services;
using GpsNote.Services.Authorization;
using GpsNote.Models;
using GpsNote.Helpers;

namespace GpsNote.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {

        #region -- Private fields --

        IPageDialogService _dialogService;
        IAuthorization _authorization;

        #endregion

        public SignInViewModel(INavigationService navigationService, IPageDialogService dialogService, IAuthorization authorization) : base(navigationService)
        {
            _dialogService = dialogService;
            _authorization = authorization;

            Title = "SignIn";      
        }


        #region -- Publics -- 

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

        public DelegateCommand OnSignUpTapCommand => new DelegateCommand(NavigationToSignUp);
        public DelegateCommand OnSignInButtonTapCommand => new DelegateCommand(AuthorizationUser, CanExecute);

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


        public override void Initialize(INavigationParameters parameters)
        {
            //base.Initialize(parameters);

            //Email = (string)parameters["newUseremail"];
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Email = (string)parameters["newUserEmail"];
        }

        #endregion



        #region Private helpers

        private bool CanExecute() => IsSignInButtonEnabled;

        private async void NavigationToSignUp()
        {
            await NavigationService.NavigateAsync(nameof(SignUpPage));
        }


        private async void AuthorizationUser()
        {
            if (!Validator.Validate(Email, Validator.patternEmail))
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                       "Invalid email. Try again",
                                                       "Cancel");
                Email = "";
                Password = "";
                return;
            }
            if (!Validator.Validate(Password, Validator.patternPassword))
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                       "Invalid password. Invalid password. Password must be from 8 to 16 characters, must contain at least one uppercase letter, one lowercase and one number",
                                                       "Cancel");
                Password = "";
                return;
            }

            User checkingUser = new User
            {
                Email = Email,
                Password = Password
            };

            bool result = await _authorization.IsAuthorization(checkingUser);

            if (!result)
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                       "Invalid login or password",
                                                       "Cancel");
                Password = "";
                return;
            }


        }

        #endregion
    }
}