using System.ComponentModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using GpsNote.Enums;
using GpsNote.Services.Authentication;
using GpsNote.Constants;


namespace GpsNote.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private IAuthenticationService _authenticationService;
        private IPageDialogService _dialogService;


        public SignUpViewModel(INavigationService navigationService, 
                               IPageDialogService dialogService, 
                               IAuthenticationService registration) : base(navigationService)
        {
            _authenticationService = registration;
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

        private DelegateCommand onSignUpCommand;
        public DelegateCommand OnSignUpCommand => onSignUpCommand ?? new DelegateCommand(OnAuthenticationUserAsync, CanExecute);

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
                if (!string.IsNullOrWhiteSpace(Name) &&
                    !string.IsNullOrWhiteSpace(Email) &&
                    !string.IsNullOrWhiteSpace(Password) &&
                    !string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    IsSignUpButtonEnabled = true;
                }
                else
                {
                    IsSignUpButtonEnabled = false;
                }
            }
        }

        #endregion


        #region -- Private helpers --

        private bool CanExecute()
        {
            return IsSignUpButtonEnabled;
        }

        private async void OnAuthenticationUserAsync()
        {
            CodeUserAuthresult result = await _authenticationService.SignUpAsync(Name,
                                                                                          Email,
                                                                                          Password,
                                                                                          ConfirmPassword);

            switch (result)
            {
                case CodeUserAuthresult.InvalidName:
                    await _dialogService.DisplayAlertAsync("Error",
                                                           "Invalid login. Login must be from 4 to 16 characters and must not start with a number",
                                                           "Cancel");
                    Name = "";
                    Password = "";
                    ConfirmPassword = "";
                    break;
                case CodeUserAuthresult.InvalidEmail:
                    await _dialogService.DisplayAlertAsync("Error",
                                                           "Invalid email. Try again",
                                                           "Cancel");
                    Email = "";
                    Password = "";
                    ConfirmPassword = "";
                    break;
                case CodeUserAuthresult.InvalidPassword:
                    await _dialogService.DisplayAlertAsync("Error",
                                                           "Invalid password. Invalid password. Password must be from 8 to 16 characters, must contain at least one uppercase letter, one lowercase and one number",
                                                         "Cancel");
                    Password = "";
                    ConfirmPassword = "";
                    break;
                case CodeUserAuthresult.PasswordMismatch:
                    await _dialogService.DisplayAlertAsync("Error",
                                                           "Passwords mismatch. Re-enter passwords",
                                                           "Cancel");
                    Password = "";
                    ConfirmPassword = "";
                    break;
                case CodeUserAuthresult.EmailTaken:
                    await _dialogService.DisplayAlertAsync("Error",
                                                           "This email is already taken. Come up with another",
                                                           "Cancel");
                    Name = "";
                    Email = "";
                    Password = "";
                    ConfirmPassword = "";
                    break;
                case CodeUserAuthresult.Passed:
                    await _dialogService.DisplayAlertAsync("SUCCESS",
                                                           "Account created!!!",
                                                           "Cancel");
                    break;
                default:
                    await _dialogService.DisplayAlertAsync("Error",
                                                           "Result unknown",
                                                           "Cancel");
                    Name = "";
                    Email = "";
                    Password = "";
                    ConfirmPassword = "";
                    break;
            }

            if(result == CodeUserAuthresult.Passed)
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    {ConstantsValue.NEW_USER_EMAIL, Email }
                };

                await NavigationService.GoBackAsync(parameters);
            }
        }

        #endregion
    }
}