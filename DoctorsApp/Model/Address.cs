using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorsApp.Model
{
    public class Address
    {
        public Address(string street, string city, string stateCode, int zip)
        {
            Street = street;
            City = city;
            StateCode = stateCode;
            Zip = Zip;
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public int Zip { get; set; }
        public string FullAddress
        {
            get
            {
                return String.Format("{0}, {1}, {2} {3}", Street, City, StateCode, Zip);
            }
        }
    }
}