using Hotel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Model
{
    public class Address
    {
        public Address(string city, string street, string postalCode, string houseNumber)
        {
            City = city;
            Street = street;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
        }
        private string _city;
        public string City { get { return _city; } set { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Mun is empty"); _city = value; } }
        private string _postalCode;
        public string PostalCode { get { return _postalCode; } set { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Zip is empty"); _postalCode = value; } }
        private string _houseNumber;
        public string HouseNumber { get { return _houseNumber; } set { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("HN is empty"); _houseNumber = value; } }
        private string _street;
        public string Street { get { return _street; } set { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Street is empty"); _street = value; } }      
    }
}
