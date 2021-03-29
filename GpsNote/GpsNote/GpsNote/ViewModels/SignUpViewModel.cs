using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GpsNote.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        public SignUpViewModel(INavigationService navigationService) : base(navigationService)
        {
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
