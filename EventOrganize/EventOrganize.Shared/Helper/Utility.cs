using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Windows.Storage;
using EventOrganize.Domain;
using Newtonsoft.Json.Linq;

namespace EventOrganize.Helper
{
    public static class Utility
    {
        private static ApplicationDataContainer cloudStorage = ApplicationData.Current.RoamingSettings;

        public static async void CreateAndUpdateTags()
        {
            double latitude = App.Lattitude;
            double longitute = App.longtitude;

            Debug.WriteLine("Updating tags started");
            //using the bing api from here

            var client = new HttpClient();
            var url = string.Format("http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?o=json&key={2}",
                latitude,
                longitute,
                "Apt41Xkk3z4iDILoiNw1a2jsu_waWdAlm2knrzsQTC3-1pQbZ80yLZR6uNZ75jIC");

            var response = await client.GetAsync(new Uri(url));
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(jsonString);
            JToken jsonResourceSets = json["resourceSets"];
            JToken jsonAddress = null;
            JToken jsonResource = null;

            if (jsonResourceSets.HasValues)
            {
                jsonResource = jsonResourceSets[0]["resources"];
            }
            if (jsonResource.HasValues)
            {
                jsonAddress = jsonResource[0]["address"];
            }

            Address address = null;

            if (jsonAddress != null)
            {
                address = new Address
                {
                    PostalAddress = (string) jsonAddress["postalCode"],
                    Locality = (string) jsonAddress["locality"],
                    County = (string) jsonAddress["adminDistrict2"],
                    State = (string) jsonAddress["adminDistrict"],
                    Country = (string) jsonAddress["countryRegion"],
                };

                App.address = address;
                Debug.WriteLine("Updating tags completed.  You are currently at zipcode: " + address.PostalAddress);
            }

            App.UpdateTags();

        }

        public static string GetValueFromCloud(string p)
        {
            return cloudStorage.Values.ContainsKey(p) ? (string)cloudStorage.Values[p] : string.Empty;
        }

        public static void RemoveFromCloud(string p)
        {
            cloudStorage.Values.Remove(p);
        }

        public static void AddToCloud(string key, string value)
        {
            RemoveFromCloud(key);
            cloudStorage.Values.Add(key, value);
        }
    }
}
