using System.ComponentModel;
using System.Threading.Tasks;
using Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using Plugin.Permissions.Abstractions;
using GpsNote.Views;
using GpsNote.Services.Authorization;
using GpsNote.Enums;
using GpsNote.Constants;
using GpsNote.Services.Authentication;


namespace GpsNote.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;
        private IAuthorizationService _authorizationService;
        private IAuthenticationService _authenticationService;


        public SignInViewModel(INavigationService navigationService, 
                               IPageDialogService dialogService, 
                               IAuthorizationService authorizationService, 
                               IAuthenticationService authenticationService) : base(navigationService)
        {
            _dialogService = dialogService;
            _authorizationService = authorizationService;
            _authenticationService = authenticationService;

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

        private DelegateCommand onSignUpTapCommand;
        public DelegateCommand OnSignUpTapCommand => onSignUpTapCommand ?? new DelegateCommand(OnNavigationToSignUpAsync);

        private DelegateCommand onSignInButtonTapCommand;
        public DelegateCommand OnSignInButtonTapCommand => onSignInButtonTapCommand ?? new DelegateCommand(OnSignInUserAsync, CanExecute);

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
            if(parameters.TryGetValue<string>(ConstantsValue.NEW_USER_EMAIL, out string email))
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
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Invalid email. Try again",
                                                       "Cancel");
                    Email = "";
                    Password = "";
                    break;
                case CodeUserAuthresult.InvalidPassword:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Invalid password. Invalid password. Password must be from 8 to 16 characters, must contain at least one uppercase letter, one lowercase and one number",
                                                       "Cancel");
                    Password = "";
                    break;
                case CodeUserAuthresult.EmailNotFound:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "This email wasn't found",
                                                       "Cancel");
                    Password = "";
                    break;
                case CodeUserAuthresult.WrongPassword:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Wrong password",
                                                       "Cancel");
                    Password = "";
                    break;
                case CodeUserAuthresult.Passed:
                    await NavigationService.NavigateAsync($"/{nameof(MainTabbedPage)}");
                    break;
                default:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Result unknown",
                                                       "Cancel");
                    Email = "";
                    Password = "";
                    break;
            }
        }

        #endregion
    }
}