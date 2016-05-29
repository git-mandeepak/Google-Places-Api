using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace GooglePlacesConsoleApp
{
    class Constants
    {
        public static string GoogleApiKey = "AIzaSyAyuAHB8qK7MA3yXQSrIQlWEYJ4CouPRec";
        public const string GoogleSearchType = "(cities)";
        public const string GoogleSearchLanguage = "en_US";
        public const string GooglePlacesURL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&types={1}&language={2}&key={3}";        
        public const string GooglePlaceDetailURL = "https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}";
        

    }

    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            GetData(client);

        }

        static void GetData(HttpClient client)
        {
            var responce = GetPlace(client);
        }

        private static PlacesWrapper GetPlace(HttpClient client)
        {
            PlacesWrapper placesWrapper = new PlacesWrapper();

            var uri = new Uri(string.Format(Constants.GooglePlacesURL, "Chan", Constants.GoogleSearchType, Constants.GoogleSearchLanguage, Constants.GoogleApiKey));
            try
            {
                client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    placesWrapper = JsonConvert.DeserializeObject<PlacesWrapper>(content);
                    foreach(var item in placesWrapper.predictions)
                    Console.WriteLine(item.description);
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }

            return placesWrapper;
        }
    }
}
