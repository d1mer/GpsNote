using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Models;

namespace GpsNote.Services.PinService
{
    public interface IPinService
    {
        Task<List<PinModel>> GetUsersPinsAsync();

        Task<Pin> GetNewPinFromPositionAsync(Position position);

        Task<int> SavePinModelToDatabaseAsync(PinModel pinModel);

        Task<int> UpdatePinModelAsync(PinModel pinModelDb);

        Task<PinModel> FindPinModelAsync(Expression<Func<PinModel, bool>> predicate);

        Task<int> DeletePinModelAsync(PinModel pinModelDb);
    }
}