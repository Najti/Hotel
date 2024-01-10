using Hotel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Model
{
    public class Organizer
    {
        public Organizer(int id, string name, ContactInfo contactInfo)
        {
            _name = name;
            _contactInfo = contactInfo;
            _id = id;
        }
        public Organizer(string name, ContactInfo contactInfo)
        {
            _name = name;
            _contactInfo = contactInfo;
        }
        private string _name;
        public string Name
        {
            get
            { return _name; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new OrganizerException("Organizer name cannot be empty."); _name = value; }
        }
        private int _id;
        public int Id
        {
            get
            { return _id; }
            set
            { if (value <= 0) throw new OrganizerException("Organizer ID cannot be less than 1."); _id = value; }
        }
        private ContactInfo _contactInfo;
        public ContactInfo Contact
        {
            get
            { return _contactInfo; }
            set
            { if (value == null) throw new OrganizerException("Organizer contact info cannot be empty."); _contactInfo = value; }
        }

    }
}
