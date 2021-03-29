using GpsNote.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GpsNote.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        
        public SignInViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "SignIn";      
        }


        #region -- Publics -- 

        private string _name = "";
        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        #endregion




        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Password) || args.PropertyName == nameof(Name))
            {
                if (Name.Length > 0 && Password.Length > 0)
                    IsSignInButtonEnabled = true;
                else
                    IsSignInButtonEnabled = false;
            }
        }

        #endregion


        #region Private helpers

        private async void NavigationToSignUp()
        {
            await NavigationService.NavigateAsync(nameof(SignUpPage));
        }

        #endregion
    }
}