using System;
using System.Threading.Tasks;
using GpsNote.Services.SettingsService;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace GpsNote.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private ISettingsManager _settingsManager;

        public PermissionsService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region -- IPermissionService implementation --

        public async Task<PermissionStatus> CheckStatusAsync<T>() where T : BasePermission, new()
        {
            PermissionStatus permissionStatus = PermissionStatus.Unknown;

            try
            {
                permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<T>();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return permissionStatus;
        }

        public async Task<PermissionStatus> RequestPermissionAsync<T>() where T : BasePermission, new()
        {
            PermissionStatus permissionStatus = PermissionStatus.Unknown;

            try
            {
                permissionStatus = await CrossPermissions.Current.RequestPermissionAsync<T>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return permissionStatus;
        }

        public async Task<bool> ShowRequestPermission(Permission permission)
        {
            bool result = false;
            try
            {
                result = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public async Task<bool> ShowRequestPermission<T>() where T : BasePermission, new()
        {
            bool result = false;
            PermissionStatus status = await CheckStatusAsync<T>();

            if (status == PermissionStatus.Granted)
            {
                result = true;
            }
            else
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<T>();
                result = status == PermissionStatus.Granted;
            }

            return result;
        }

        public bool CheckLocationPermission()
        {
            return _settingsManager.LocationPermission;
        }

        #endregion
    }
}