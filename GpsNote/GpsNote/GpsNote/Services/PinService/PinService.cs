using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.UserService;
using GpsNote.Models;
using GpsNote.Services.SettingsService;
using System.Threading.Tasks;
using System.Linq;

namespace GpsNote.Services.PinService
{
    public class PinService : IPinService
    {
        #region --Private fields --

        IRepositoryService _repositoryService;
        ISettingsService   _settingsService;

        #endregion


        #region -- Constructor --

        public PinService(IRepositoryService repositoryService, ISettingsService settingsService)
        {
            _repositoryService = repositoryService;
            _settingsService   = settingsService;
        }

        #endregion


        #region -- Implement interface IPinService --

        public List<Pin> GetUserPins()
        {
            if (_settingsService.IdCurrentUser == -1)
                return null;

            List<PinModelDb> pinModels = _repositoryService.GetAllAsync<PinModelDb>(p => p.Owner == _settingsService.IdCurrentUser).Result;
            List<Pin> pins = new List<Pin>();

            foreach (PinModelDb pinModel in pinModels)
            {
                Pin pin = new Pin();
                pin.Position = new Position(pinModel.Latitude, pinModel.Longitude);
                pin.Label = pinModel.Label;
                pin.Address = pinModel.Address;
                pins.Add(pin);
            }

            return pins;
        }


        public List<PinModelDb> GetUserPinModels()
        {
            if (_settingsService.IdCurrentUser == -1)
                return null;

            List<PinModelDb> pinModels = _repositoryService.GetAllAsync<PinModelDb>(p => p.Owner == _settingsService.IdCurrentUser).Result;

            return pinModels;
        }


        public async Task<Pin> GetNewPinAsync(Position position)
        {
            Geocoder geocoder = new Geocoder();
            IEnumerable<string> addresses = await geocoder.GetAddressesForPositionAsync(position);

            return await Task.Run(() => GetPin(position, addresses));
        }


        public void SavePinModelToDatabase(PinModelDb pinModel) => _repositoryService.InsertAsync<PinModelDb>(pinModel);

        #endregion

        #region -- Private helpers --

        private Pin GetPin(Position position, IEnumerable<string> addresses)
        {
            Pin pin = new Pin
            {
                Position = position,
                Address = addresses != null ? addresses.FirstOrDefault() : string.Empty,
                Label = addresses != null ?
                       addresses.FirstOrDefault().Substring(0, addresses.FirstOrDefault().IndexOf(",") != -1 ?
                                                             addresses.FirstOrDefault().IndexOf(",") :
                                                             addresses.FirstOrDefault().Length - 1) :
                       "New pin"
            };
            return pin;
        }

        #endregion
    }
}