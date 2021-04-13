using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Services.RepositoryService;
using GpsNote.Models;
using GpsNote.Services.SettingsService;
using GpsNote.Services.Authorization;

namespace GpsNote.Services.PinService
{
    public class PinService : IPinService
    {
        #region --Private fields --

        IRepositoryService _repositoryService;
        ISettingsManager   _settingsService;
        IAuthorizationService _authorizationService;

        #endregion


        #region -- Constructor --

        public PinService(IRepositoryService repositoryService, ISettingsManager settingsService, IAuthorizationService authorizationService)
        {
            _repositoryService = repositoryService;
            _settingsService   = settingsService;
            _authorizationService = authorizationService;
        }

        #endregion


        #region -- Implement interface IPinService --

        public bool IsDisplayConcretePin 
        {
            get => _settingsService.ShowPin;
            set => _settingsService.ShowPin = value; 
        }

        public async Task<List<PinModel>> GetUsersPinsAsync()
        {
            List<PinModel> userPins = null;
            
            if (_authorizationService.GetCurrentUserID() != -1)
            {
                try
                {
                    userPins = await _repositoryService.GetAllAsync<PinModel>(p => p.Owner == _settingsService.AuthorizedUserID);
                }
                catch(Exception ex)
                {
                    // handle exception
                }
            }

            return userPins;
        }

        public async Task<int> SavePinModelToDatabaseAsync(PinModel pinModel)
        {
            int rows = 0;

            try
            {
                rows = await _repositoryService.InsertAsync<PinModel>(pinModel);
            }
            catch(Exception ex) 
            {
                rows = 0;
            }

            return rows;
        }

        public async Task<int> UpdatePinModelAsync(PinModel pinModel)
        {
            int index = 0;

            try
            {
                index = await _repositoryService.UpdateAsync<PinModel>(pinModel);
            }
            catch(Exception ex)
            {
                index = -1;
            }

            return index;
        }

        public async Task<PinModel> FindPinModelAsync(Expression<Func<PinModel, bool>> predicate)
        {
            PinModel pinModel = null;

            try
            {
                pinModel = await _repositoryService.FindEntity<PinModel>(predicate);
            }
            catch(Exception ex) { }

            return pinModel;
        }

        public async Task<int> DeletePinModelAsync(PinModel pinModel)
        {
            int rows = 0;
            try
            {
                rows = await _repositoryService.DeleteAsync<PinModel>(pinModel);
            }
            catch(Exception ex)
            {
                rows = -1;
            }

            return rows;
        }

        public async Task<Pin> GetNewPinFromPositionAsync(Position position)
        {
            Geocoder geocoder = new Geocoder();
            IEnumerable<string> addresses = null;
            Pin pin;

            try
            {
                addresses = await geocoder.GetAddressesForPositionAsync(position);
                pin = await Task.Run(() => GetPin(position, addresses));
            }
            catch(Exception ex) 
            {
                pin = null;
            }

            return pin;
        }

        #endregion


        #region -- Private helpers --

        private Pin GetPin(Position position, IEnumerable<string> addresses)
        {
            Pin pin = new Pin
            {
                Position = position,
                Address = addresses != null ? addresses.FirstOrDefault() : string.Empty,
                Label = addresses != null && addresses.FirstOrDefault() != null ?
                       addresses.FirstOrDefault().Substring(0, addresses.FirstOrDefault().IndexOf(",") != -1 ?
                                                             addresses.FirstOrDefault().IndexOf(",") :
                                                             addresses.FirstOrDefault().Length) :
                       "New pin"
            };
            return pin;
        }

        #endregion
    }
}