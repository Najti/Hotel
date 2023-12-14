using Hotel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hotel.Domain.Model
{
    public class ContactInfo
    {
        private int _id;
        private string _email;
        private string _phone;
        private Address _address;

        public ContactInfo(string email, string phone, Address address)
        {
            _email = email;
            _phone = phone;
            _address = address;
        }
        public ContactInfo(int id, string email, string phone, Address address)
        {
            _id = id;
            _email = email;
            _phone = phone;
            _address = address;
        }
        public int Id
        {
            get { return _id; } set {_id=value; }
        }
        public string Email
        {
            get
            { return _email; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Email cannot be empty or spaces.");if (!value.Contains("@")) throw new CustomerException("Email is invalid"); _email = value; }
        }

        public string Phone
        {
            get
            { return _phone; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Phone cannot be empty or spaces."); _phone = value; }
        }

        public Address Address
        {
            get { return _address; }
            set { if (value == null) throw new CustomerException("Address cannot be empty."); _address = value; }
        }
    }
}
