using GpsNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.PinService
{
    public interface IPinService
    {
        List<Pin> GetUserPins();

        Task<Pin> GetNewPinAsync(Position position);

        void SavePinModelToDatabase(PinModel pinModel);
    }
}
