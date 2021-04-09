using GpsNote.Views;
using Prism.Commands;
using Prism.Navigation;
using System.ComponentModel;
using Prism.Services;
using GpsNote.Services.AuthorizationService;
using GpsNote.Enums;

namespace GpsNote.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {

        #region -- Private fields --

        IPageDialogService _dialogService;
        IAuthorizeService _authorizeService;

        #endregion

        public SignInViewModel(INavigationService navigationService, IPageDialogService dialogService, IAuthorizeService authorizationService) : base(navigationService)
        {
            _dialogService = dialogService;
            _authorizeService = authorizationService;

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
        public DelegateCommand OnSignInButtonTapCommand => new DelegateCommand(AuthorizeUser, CanExecute);

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


        private async void AuthorizeUser()
        {
            CodeUserAuthresult result = await _authorizeService.Authorize(Email, Password);

            switch (result)
            {
                case CodeUserAuthresult.InvalidEmail:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Invalid email. Try again",
                                                       "Cancel");
                    Email = "";
                    Password = "";
                    return;
                case CodeUserAuthresult.InvalidPassword:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Invalid password. Invalid password. Password must be from 8 to 16 characters, must contain at least one uppercase letter, one lowercase and one number",
                                                       "Cancel");
                    Password = "";
                    return;
                case CodeUserAuthresult.EmailNotFound:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "This email wasn't found",
                                                       "Cancel");
                    Password = "";
                    return;
                case CodeUserAuthresult.WrongPassword:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Wrong password",
                                                       "Cancel");
                    Password = "";
                    return;
                case CodeUserAuthresult.Passed:
                    await NavigationService.NavigateAsync($"/{nameof(MainTabbedPage)}");
                    break;
                default:
                    await _dialogService.DisplayAlertAsync("Error",
                                                       "Result unknown",
                                                       "Cancel");
                    Email = "";
                    Password = "";
                    return;
            }
        }

        #endregion
    }
}