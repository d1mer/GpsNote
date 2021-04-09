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

            List<PinModel> pinModels = _repositoryService.GetAllAsync<PinModel>(p => p.Owner == _settingsService.IdCurrentUser).Result;
            List<Pin> pins = new List<Pin>();

            foreach (PinModel pinModel in pinModels)
            {
                Pin pin = new Pin();
                pin.Position = new Position(pinModel.Latitude, pinModel.Longitude);
                pin.Label = pinModel.Label;
                pin.Address = pinModel.Address;
                pins.Add(pin);
            }

            return pins;
        }


        public List<PinModel> GetUserPinModels()
        {
            if (_settingsService.IdCurrentUser == -1)
                return null;

            List<PinModel> pinModels = _repositoryService.GetAllAsync<PinModel>(p => p.Owner == _settingsService.IdCurrentUser).Result;

            return pinModels;
        }


        public async Task<Pin> GetNewPinAsync(Position position)
        {
            Geocoder geocoder = new Geocoder();
            IEnumerable<string> address = await geocoder.GetAddressesForPositionAsync(position);

            Pin pin = new Pin
            {
                Position = position,
                Address = address != null ? address.FirstOrDefault() : string.Empty,
                Label = address != null ?
                        address.FirstOrDefault().Substring(0, address.FirstOrDefault().IndexOf(",") != -1 ?
                                                              address.FirstOrDefault().IndexOf(",") :
                                                              address.FirstOrDefault().Length - 1) :
                        "New pin"
            };
            return pin;
        }


        public void SavePinModelToDatabase(PinModel pinModel) => _repositoryService.InsertAsync<PinModel>(pinModel);

        #endregion
    }
}