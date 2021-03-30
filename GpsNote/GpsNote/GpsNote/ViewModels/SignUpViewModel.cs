using GpsNote.Helpers;
using GpsNote.Models;
using GpsNote.Services.Registration;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GpsNote.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        #region -- Private fields --

        IRegistration _registration;
        IPageDialogService _dialogService;

        #endregion


        public SignUpViewModel(INavigationService navigationService, IPageDialogService dialogService, IRegistration registration) : base(navigationService)
        {
            _registration = registration;
            _dialogService = dialogService;

            Title = "SignUp";
        }


        #region -- Publics --

        private string _name = "";
        public string Name 
        { 
            get => _name;
            set => SetProperty(ref _name, value);
        }

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

        private string _confirmPassword = "";
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private bool _isSignUpButtonEnabled = false;
        public bool IsSignUpButtonEnabled 
        {
            get => _isSignUpButtonEnabled;
            set => SetProperty(ref _isSignUpButtonEnabled, value);
        }


        public DelegateCommand OnSignUpCommand => new DelegateCommand(RegistrationUser, CanExecute);

        #endregion



        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Name) ||
                args.PropertyName == nameof(Email) ||
                args.PropertyName == nameof(Password) ||
                args.PropertyName == nameof(ConfirmPassword))
            {
                if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                    IsSignUpButtonEnabled = true;
                else
                    IsSignUpButtonEnabled = false;
            }
        }

        #endregion



        #region -- Private helpers --

        private bool CanExecute() => IsSignUpButtonEnabled;

        private async void RegistrationUser()
        {
            Name = Name.Trim();
            Email = Email.Trim();
            Password = Password.Trim();
            ConfirmPassword = ConfirmPassword.Trim();

            if(!Validator.Validate(Name, Validator.patternName))
            {
                await _dialogService.DisplayAlertAsync("Error", 
                                                       "Invalid login. Login must be from 4 to 16 characters and must not start with a number", 
                                                       "Cancel");
                Name = "";
                Password = "";
                ConfirmPassword = "";
                return;
            }

            if (!Validator.Validate(Email, Validator.patternEmail))
            {
                await _dialogService.DisplayAlertAsync("Error", 
                                                       "Invalid email. Try again", 
                                                       "Cancel");
                Email = "";
                Password = "";
                ConfirmPassword = "";
                return;
            }

            if (!Validator.Validate(Password, Validator.patternPassword))
            {
                await _dialogService.DisplayAlertAsync("Error", 
                                                       "Invalid password. Invalid password. Password must be from 8 to 16 characters, must contain at least one uppercase letter, one lowercase and one number", 
                                                       "Cancel");
                Password = "";
                ConfirmPassword = "";
                return;
            }

            if(Password != ConfirmPassword)
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                       "Passwords mismatch. Re-enter passwords",
                                                       "Cancel");
                Password = "";
                ConfirmPassword = "";
                return;
            }

            User newUser = new User
            {
                Name = Name,
                Email = Email,
                Password = Password
            };

            bool result = await _registration.IsRegistration(newUser);

            if (!result)
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                       "This email is already taken. Come up with another",
                                                       "Cancel");
                Name = "";
                Email = "";
                Password = "";
                ConfirmPassword = "";
                return;
            }

            await _dialogService.DisplayAlertAsync("SUCCESS",
                                                       "Account created!!!",
                                                       "Cancel");

            NavigationParameters parameters = new NavigationParameters
            {
                {"newUserEmail", newUser.Email }
            };

            await NavigationService.GoBackAsync(parameters);
            
           
        }

        #endregion
    }
}