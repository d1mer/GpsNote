using GpsNote.Services.SettingsService;

namespace GpsNote.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISettingsManager _settingsManager;


        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }


        #region -- IAuthorizeService implementation --

        public int GetCurrentUserID()
        {
            return _settingsManager.AuthorizedUserID;
        }

        public bool IsAuthorized()
        {
            return _settingsManager.AuthorizedUserID >= 0;
        }

        public void LogOut()
        {
            _settingsManager.CleanUpAuthorizedUser();
        }

        #endregion
    }
}