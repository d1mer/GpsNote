using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace GpsNote.Services.Permissions
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckStatusAsync<T>() where T : BasePermission, new();

        Task<PermissionStatus> RequestPermissionAsync<T>() where T : BasePermission, new();

        Task<bool> ShowRequestPermission(Permission permission);

        Task<bool> ShowRequestPermission<T>() where T : BasePermission, new();

        bool CheckLocationPermission();
        void SaveLocationPermission(bool value);
    }
}