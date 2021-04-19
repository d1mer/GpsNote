using GpsNote.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.TimeZone
{
    public class TimeZoneService : ITimeZoneService
    {
        #region -- ITimeZoneService implementation --

        public async Task<TimeZoneResponse> GetTimeZoneAsync(Position position)
        {
            TimeZoneResponse timeZoneResponse = null;
            NetworkAccess networkAccess = Connectivity.NetworkAccess;
            
            if(networkAccess == NetworkAccess.Internet)
            {
                string requestString = BuildRequest(position);

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(requestString);
                request.Method = HttpMethod.Get;

                HttpResponseMessage response = await client.SendAsync(request);

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    var json = await content.ReadAsStringAsync();

                    timeZoneResponse = JsonSerializer.Deserialize<TimeZoneResponse>(json);
                }
            }

            return timeZoneResponse;
        }

        #endregion

        #region -- Private helpers --

        private string BuildRequest(Position position)
        {
            StringBuilder request = new StringBuilder(Constants.BASE_URI_TIMEZONE_API);

            request.Append($"location={position.Latitude},{position.Longitude}&");
            request.Append($"timestamp={GetTimeStamp()}&");
            request.Append($"key={Constants.GPSNOTE_KEY}");

            return request.ToString();
        }

        private long GetTimeStamp()
        {
            DateTimeOffset offset = new DateTimeOffset(DateTime.Now);
            return offset.ToUnixTimeSeconds();
        }

        #endregion
    }
}