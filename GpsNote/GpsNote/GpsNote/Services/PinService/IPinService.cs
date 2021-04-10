using GpsNote.Models;
using GpsNote.ViewModels.ExtentedViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.PinService
{
    public interface IPinService
    {
        Task<List<Pin>> GetUserPinModelDbToPinsFromDatabaseAsync();

        Task<List<PinModelDb>> GetUserPinModelsFromDatabaseAsync();

        Task<Pin> GetNewPinFromPositionAsync(Position position);

        Task<int> SavePinModelDbToDatabaseAsync(PinModelDb pinModel);

        Task<int> UpdatePinModelDbAsync(PinModelDb pinModelDb);

        Task<int> UpdatePinModelDbAsync(PinViewModel pinViewModel);

        Task<PinModelDb> FindPinModelDbAsync(Expression<Func<PinModelDb, bool>> predicate);

        Task<int> DeletePinModelDbAsync(PinModelDb pinModelDb);

        Task<int> DeletePinModelDbAsync(PinViewModel pinViewModel);
    }
}
