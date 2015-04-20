﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using EventOrganize.Domain;
using Newtonsoft.Json.Linq;

namespace EventOrganize.Helper
{
    public static class Utility
    {
        public static async void CreateAndUpdateTags(double latitude, double longitute)
        {
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

            if (jsonAddress == null)
            {
                return;
            }

            var address = new Address
            {
                PostalAddress = (string)jsonAddress["postalCode"],
                Locality = (string)jsonAddress["locality"],
                County = (string)jsonAddress["adminDistrict2"],
                State = (string)jsonAddress["adminDistrict"],
                Country = (string)jsonAddress["countryRegion"],
            };

            App.address = address;
            App.UpdateTags();

            Debug.WriteLine("Updating tags completed");
        }
    }
}
