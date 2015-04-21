using System;
using Microsoft.WindowsAzure.Mobile.Service;

namespace EventOrganizeService.DataObjects
{
    public class OrganizeEvent : EntityData
    {
        public string LeaderID { get; set; }
        public double locationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public string JoinID { get; set; }
        public int ZipCode { get; set; }
        public string Name { get; set; }
    }
}