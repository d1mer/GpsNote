using System.ComponentModel;
using Prism.Commands;
using Prism.Navigation;
using GpsNote.Enums;
using GpsNote.Services.Localization;
using GpsNote.Services.GoogleAuthentication;
using GpsNote.Views;
using GpsNote.Helpers;
using GpsNote.Services.Authentication;

namespace GpsNote.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly IGoogleAuthenticationService _googleAuthenticationService;
        private readonly IAuthenticationService _authenticationService;


        public SignUpViewModel(INavigationService navigationService,
                               ILocalizationService localizationService,
                               IGoogleAuthenticationService googleAuthenticationService,
                               IAuthenticationService authenticationService) : base(navigationService, localizationService)
        {
            _googleAuthenticationService = googleAuthenticationService;
            _authenticationService = authenticationService;

            Title = Resource["CreateAccountTitle"];
        }


        #region -- Public properties --

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

        private string errorTextEmail;
        public string ErrorTextEmail
        {
            get => errorTextEmail;
            set => SetProperty(ref errorTextEmail, value);
        }

        private string errorTextName;
        public string ErrorTextName
        {
            get => errorTextName;
            set => SetProperty(ref errorTextName, value);
        }

        private bool tapEmailImage;
        public bool TapEmailImage
        {
            get => tapEmailImage;
            set => SetProperty(ref tapEmailImage, value);
        }

        private bool tapNameImage;
        public bool TapNameImage
        {
            get => tapNameImage;
            set => SetProperty(ref tapNameImage, value);
        }

        private bool _isNextButtonEnabled = false;
        public bool IsNextButtonEnabled
        {
            get => _isNextButtonEnabled;
            set => SetProperty(ref _isNextButtonEnabled, value);
        }

        private DelegateCommand nextButtonTapCommand;
        public DelegateCommand NextButtonTapCommand => nextButtonTapCommand ?? new DelegateCommand(OnNextPageAsync, CanExecute);

        private DelegateCommand onSignInGoogleButtonTapCommand;
        public DelegateCommand OnSignInGoogleButtonTapCommand => onSignInGoogleButtonTapCommand ?? new DelegateCommand(OnSignInGoogleUser);

        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);

        #endregion


        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Name) ||
                args.PropertyName == nameof(Email))
            {
                if (!string.IsNullOrWhiteSpace(Name) &&
                    !string.IsNullOrWhiteSpace(Email))
                {
                    IsNextButtonEnabled = true;
                }
                else
                {
                    IsNextButtonEnabled = false;
                }
            }
            else if (args.PropertyName == nameof(TapEmailImage))
            {
                Email = string.Empty;
            }
            else if (args.PropertyName == nameof(TapNameImage))
            {
                Name = string.Empty;
            }
        }

        #endregion


        #region -- Private helpers --

        private bool CanExecute()
        {
            return IsNextButtonEnabled;
        }

        private void OnBackPressed()
        {
            NavigationService.NavigateAsync($"/{nameof(MainPage)}");
        }

        private void OnSignInGoogleUser()
        {
            _googleAuthenticationService.SignInWithGoogle();
        }

        private async void OnNextPageAsync()
        {
            Name = Name.Trim();
            Email = Email.Trim();

            bool verifyEmail = Validator.Validate(Email, VerifyEntity.Email);
            bool verifyName = Validator.Validate(Name, VerifyEntity.Name);

            if (verifyEmail && verifyName)
            {
                bool takenEmail = await _authenticationService.OnEmailTakenAsync(Email);

                if (!takenEmail)
                {
                    (string, string) userData = (Email, Name);
                    NavigationParameters parameter = new NavigationParameters();
                    parameter.Add(Constants.DATA_USER, userData);
                    await NavigationService.NavigateAsync(nameof(SignUpPage2), parameter);
                }
                else
                {
                    ErrorTextEmail = Resource["TakenEmailError"];
                }
            }
            else
            {
                ErrorTextEmail = verifyEmail ?
                                 string.Empty :
                                 Resource["WrongEmailError"];

                ErrorTextName = verifyName ?
                                string.Empty :
                                Resource["WrongNameError"];
            }
        }

        #endregion
    }
}