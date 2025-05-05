using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;
using TMQ.HttpClientBase;

namespace TMQ.GoogleService
{
    public class GeoLocationService
    {
        private readonly IHttpClient _httpClient;

        public GeoLocationService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RDistanceGeocoding> Geocoding(string address, string apiKeyGoogle, string googleMapUrl)
        {
            //var query = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={apiKeyGoogle}";
            var query = $"{googleMapUrl}{address}&key={apiKeyGoogle}";
            var response = await _httpClient.Get(query);
            if (response.IsSuccessStatusCode)
            {
                var responseObject = Serialize.JsonDeserializeObject<RDistanceGeocoding>(await response.Content.ReadAsStringAsync());
                return responseObject;
            }
            throw new Exception($"Can't connect to: {query}. {response.StatusCode} - {response.ReasonPhrase}");
        }
    }
    public class RDistanceGeocoding
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }
}
