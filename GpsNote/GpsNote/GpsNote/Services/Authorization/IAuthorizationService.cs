namespace GpsNote.Services.Authorization
{
    public interface IAuthorizationService
    {
        bool IsAuthorized();

        void LogOut();

        int GetCurrentUserID();
    }
}