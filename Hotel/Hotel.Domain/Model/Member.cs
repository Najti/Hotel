﻿using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Model
{
    public class Member
    {
        public Member(string name, DateOnly birthday)
        {
            Name = name;
            Birthday = birthday;
        }
        private string _name;
        public string Name
        {
            get
            { return _name; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Member name cannot be empty."); _name = value; }
        }
        private DateOnly _birthday;
        public DateOnly Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                if (DateOnly.FromDateTime(DateTime.Now) <= value) throw new CustomerException("Member birthday cannot be now or in the future.");
                _birthday = value;
            }
        }
    }
}