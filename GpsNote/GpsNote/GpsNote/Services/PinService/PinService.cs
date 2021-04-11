using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Services.RepositoryService;
using GpsNote.Models;
using GpsNote.Services.SettingsService;
using System.Threading.Tasks;
using System.Linq;
using GpsNote.Extensions;
using GpsNote.ViewModels.ExtentedViewModels;
using System.Linq.Expressions;

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

        public bool IsDisplayConcretePin 
        {
            get => _settingsService.ShowPin;
            set => _settingsService.ShowPin = value; 
        }


        public async Task<List<Pin>> GetUserPinModelDbToPinsFromDatabaseAsync()
        {
            if (_settingsService.IdCurrentUser == -1)
                return null;

            List<PinModelDb> pinModels = await _repositoryService.GetAllAsync<PinModelDb>(p => p.Owner == _settingsService.IdCurrentUser);
            List<Pin> pins = new List<Pin>();

            foreach (PinModelDb pinModelDb in pinModels)
            {
                pins.Add(pinModelDb.PinModelDbToPin());
            }

            return pins;
        }


        public async Task<List<PinModelDb>> GetUserPinModelsFromDatabaseAsync()
        {
            if (_settingsService.IdCurrentUser == -1)
                return null;

            List<PinModelDb> pinModels = await _repositoryService.GetAllAsync<PinModelDb>(p => p.Owner == _settingsService.IdCurrentUser);

            return pinModels;
        }


        public async Task<Pin> GetNewPinFromPositionAsync(Position position)
        {
            Geocoder geocoder = new Geocoder();
            IEnumerable<string> addresses = await geocoder.GetAddressesForPositionAsync(position);

            return await Task.Run(() => GetPin(position, addresses));
        }


        public async Task<int> SavePinModelDbToDatabaseAsync(PinModelDb pinModel) => 
            await _repositoryService.InsertAsync<PinModelDb>(pinModel);


        public async Task<int> UpdatePinModelDbAsync(PinModelDb pinModelDb) => 
            await _repositoryService.UpdateAsync<PinModelDb>(pinModelDb);


        public async Task<int> UpdatePinModelDbAsync(PinViewModel pinViewModel)
        {
            PinModelDb pinModelDb = pinViewModel.PinViewModelToPinModelDb();
            return await UpdatePinModelDbAsync(pinModelDb);
        }


        public async Task<PinModelDb> FindPinModelDbAsync(Expression<Func<PinModelDb, bool>> predicate) =>
            await _repositoryService.FindEntity<PinModelDb>(predicate);


        public async Task<int> DeletePinModelDbAsync(PinModelDb pinModelDb) => 
            await _repositoryService.DeleteAsync<PinModelDb>(pinModelDb);


        public async Task<int> DeletePinModelDbAsync(PinViewModel pinViewModel)
        {
            PinModelDb pinModelDb = pinViewModel.PinViewModelToPinModelDb();
            return await DeletePinModelDbAsync(pinModelDb);
        }

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