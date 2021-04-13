using GpsNote.Services.SettingsService;

namespace GpsNote.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        #region -- Private fields --

        private readonly ISettingsManager _settingsManager;

        #endregion


        #region -- Constructor --

        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #endregion


        #region -- IAuthorizeService implementation --

        public int GetCurrentUserID() => _settingsManager.AuthorizedUserID;

        public bool IsAuthorized() => _settingsManager.AuthorizedUserID >= 0;

        public void LogOut() => _settingsManager.AuthorizedUserID = -1;

        #endregion
    }
}