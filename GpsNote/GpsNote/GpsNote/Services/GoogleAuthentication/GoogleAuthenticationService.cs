using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Prism.Navigation;
using GpsNote.Services.GoogleAuthentication.AuthHelpers;
using GpsNote.Services.SettingsService;
using GpsNote.Models;
using GpsNote.Services.RepositoryService;
using GpsNote.Views;

namespace GpsNote.Services.GoogleAuthentication
{
    public class GoogleAuthenticationService : IGoogleAuthenticationService
    {
        private Account account;
        private ISettingsManager _settingsManager;
        private IRepositoryService _repositoryService;
        private INavigationService _navigationService;

        public GoogleAuthenticationService(ISettingsManager settingsManager, 
                                           IRepositoryService repositoryService,
                                           INavigationService navigationService)
        {
            _settingsManager = settingsManager;
            _repositoryService = repositoryService;
            _navigationService = navigationService;
        }

        #region -- IGoogleAuthenticationService implementation --

        public void SignInWithGoogle()
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.IOS_CLIENT_ID;
                    redirectUri = Constants.IOS_REDIRECT_URL;
                    break;
                case Device.Android:
                    clientId = Constants.ANDROID_CLIENT_ID;
                    redirectUri = Constants.ANDROID_REDIRECT_URL;
                    break;
            }

            OAuth2Authenticator authentificator = GetAuthenticator(clientId, redirectUri);

            authentificator.Completed += OnAuthCompleted;
            authentificator.Error += OnAuthError;

            AuthenticationState.Authenticator = authentificator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authentificator);
        }

        #endregion


        #region -- Private helpers --

        private async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            OAuth2Authenticator authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            GoogleUser googleUser = null;

            if (e.IsAuthenticated)
            {
                OAuth2Request request = new OAuth2Request("GET", new Uri(Constants.USER_INFO_URL), null, e.Account);
                var response = await request.GetResponseAsync();

                if (response != null)
                {
                    string userJson = await response.GetResponseTextAsync();
                    googleUser = JsonSerializer.Deserialize<GoogleUser>(userJson);
                }

                if(googleUser != null)
                {
                    int id = await IsGoogleUserExistAsync(googleUser.Email);

                    if (id == -1)
                    {
                        id = await SaveGoogleUserAsync(googleUser.Email);
                    }

                    if(id != -1)
                    {
                        _settingsManager.AuthorizedUserID = id;
                        await _navigationService.NavigateAsync($"/{nameof(MainTabbedPage)}");
                    }
                }
            }
        }

        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            OAuth2Authenticator authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }

        private OAuth2Authenticator GetAuthenticator(string clientID, string redirectURI)
        {
            OAuth2Authenticator authentificator = new OAuth2Authenticator(clientID,
                                                         null,
                                                         Constants.SCOPE,
                                                         new Uri(Constants.AUTHORIZE_URL),
                                                         new Uri(redirectURI),
                                                         new Uri(Constants.ACCESS_TOKEN_URL),
                                                         null,
                                                         true);
            return authentificator;
        }

        private async Task<int> IsGoogleUserExistAsync(string email)
        {
            int id = -1;
            User user = null;

            try
            {
                user = await _repositoryService.FindEntity<User>(u => u.Email == email);
            }
            catch (Exception ex)
            {
                id = -1;
                Console.WriteLine(ex.Message);
            }

            if(user != null)
            {
                id = user.Id;
            }
            else
            {
                id = -1;
            }

            return id;
        }

        private async Task<int> SaveGoogleUserAsync(string email)
        {
            int id = -1;
            User user = new User
            {
                Email = email
            };

            int rows = 0;

            try
            {
                rows = await _repositoryService.InsertAsync<User>(user);

                if (rows > 0)
                {
                    user = await _repositoryService.FindEntity<User>(u => u.Email == email);

                    if(user != null)
                    {
                        id = user.Id;
                    }
                }
                else
                {
                    id = -1;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return id;
        }

        #endregion
    }
}