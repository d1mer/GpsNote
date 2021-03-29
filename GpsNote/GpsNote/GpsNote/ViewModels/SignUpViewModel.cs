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

        private string _name;
        public string Name 
        { 
            get => _name;
            set => SetProperty(ref _name, value);
        }

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private bool _isSignUpButtonEnabled;
        public bool IsSignUpButtonEnabled 
        {
            get => _isSignUpButtonEnabled;
            set => SetProperty(ref _isSignUpButtonEnabled, value);
        }

        #endregion


        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.PropertyName == nameof(Name) || 
                args.PropertyName == nameof(Email) ||
                args.PropertyName == nameof(Password) ||
                args.PropertyName == nameof(ConfirmPassword))
            {
                if (Name.Length > 0 && Email.Length > 0 && Password.Length > 0 && ConfirmPassword.Length > 0)
                    IsSignUpButtonEnabled = true;
                else
                    IsSignUpButtonEnabled = false;
            }
        }

        #endregion
    }
}
