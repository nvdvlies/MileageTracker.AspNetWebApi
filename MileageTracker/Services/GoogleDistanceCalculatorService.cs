using MileageTracker.Infrastructure.DistanceCalculation;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace MileageTracker.Services {
    public class GoogleDistanceCalculatorService : IDistanceCalculatorService {
        private const string DistanceMatrixUrl = "https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}&destinations={1}&mode={2}&units={3}&language=us-en&sensor=false";

        public int GetDistance(Address origin, Address destination, Mode mode, Units units) {
            if (origin == null || destination == null) {
                return 0;
            }
            var request =
                (HttpWebRequest)
                    WebRequest.Create(
                        new Uri(String.Format(DistanceMatrixUrl,
                            new object[] {
                                HttpUtility.JavaScriptStringEncode(origin.PostalCode), 
                                HttpUtility.JavaScriptStringEncode(destination.PostalCode), 
                                Enum.GetName(typeof(Mode), mode),
                                Enum.GetName(typeof(Units), units)
                            })));
            var response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK) return 0;

            using (var streamReader = new StreamReader(response.GetResponseStream())) {
                var result = streamReader.ReadToEnd();
                if (string.IsNullOrEmpty(result)) return 0;
                var jsonObject = JObject.Parse(result);
                var distance = (int)jsonObject.SelectToken("rows[0].elements[0].distance.value");
                return distance / 1000;
            }
        }

    }
}