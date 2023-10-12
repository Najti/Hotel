using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Presentation.Customer.Model
{
    public class CustomerUI
    {
        public CustomerUI(string name, string email, string address, string phone, int nrOfMembers)
        {
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            NrOfMembers = nrOfMembers;
        }

        public CustomerUI(int? id, string name, string email, string address, string phone, int nrOfMembers)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            Phone = phone;
            NrOfMembers = nrOfMembers;
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int NrOfMembers { get; set; }
    }
}
