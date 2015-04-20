using System;
using System.Collections.Generic;
using System.Text;

namespace EventOrganize.Domain
{
    public class Address
    {
        public string PostalAddress { get; set; }
        public string Locality { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
