using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EventOrganize.Domain
{
    public class OrganizeEvent
    {
        [JsonProperty(PropertyName = "ID")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "LeaderID")]
	    public string LeaderID  { get; set; }

        [JsonProperty(PropertyName = "locationLatitude")]
        public double locationLatitude { get; set; }

        [JsonProperty(PropertyName = "LocationLongitude")]
        public double LocationLongitude { get; set; }

        //[JsonProperty(PropertyName = "StartDate")]
        //public DateTime StartDate { get; set; }

        //[JsonProperty(PropertyName = "EndDate")]
        //public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "JoinID")]
        public string JoinID { get; set; }

        [JsonProperty(PropertyName = "ZipCode")]
        public int ZipCode { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

    }
}
