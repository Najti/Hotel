﻿using Hotel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Model
{
    public class Address
    {
        private const char splitChar = '|';
        public Address(string city, string street, string postalCode, string houseNumber)
        {
            City = city;
            Street = street;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
        }

        public Address(string addressLine)
        {
            string[] parts = addressLine.Split(splitChar);

            if (parts.Length >= 1)
            {
                City = parts[0];
            }

            if (parts.Length >= 2)
            {
                PostalCode = parts[1];
            }

            if (parts.Length >= 3)
            {
                Street = parts[2];
            }

            if (parts.Length >= 4)
            {
                HouseNumber = parts[3];
            }
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _city;
        public string City
        {
            get
            { return _city; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("City cannot be empty."); _city = value; }
        }
        private string _postalCode;
        public string PostalCode
        {
            get
            { return _postalCode; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Zipcode cannot be empty."); _postalCode = value; }
        }
        private string _houseNumber;
        public string HouseNumber
        {
            get
            { return _houseNumber; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Housenumber cannot be empty."); _houseNumber = value; }
        }
        private string _street;
        public string Street
        {
            get
            { return _street; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Street is empty"); _street = value; }
        }

        public override string ToString()
        {
            return $"{City} [{PostalCode}] - {Street} - {HouseNumber}";
        }
        public string ToAddressLine()
        {
            return $"{City}{splitChar}{PostalCode}{splitChar}{Street}{splitChar}{HouseNumber}";
        }
    }
}
