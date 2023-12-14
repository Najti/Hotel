using Hotel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Model
{
    public class Customer
    {
        private string _name;
        public int Id { get; set; }
        public string Name { get { return _name; } set { if (value.Length > 500 || string.IsNullOrWhiteSpace(value)) throw new CustomerException("Customer name is invalid");_name = value; } }
        public ContactInfo Contact { get; set; }
        private List<Member> _members = new List<Member>();

        public Customer(int id, string name, ContactInfo contact)
        {
            Id = id;
            Name = name;
            Contact = contact;
        }

        public Customer(string name, ContactInfo contact)
        {
            Name = name;
            Contact = contact;
        }

        public IReadOnlyList<Member> GetMembers() { return _members.AsReadOnly(); }
        public void AddMember(Member member)
        {
            if (!_members.Contains(member))
                _members.Add(member);
            else
                throw new CustomerException("Error while adding member - Member already exists");
        }
        public void RemoveMember(Member member) 
        {
            if (_members.Contains(member))
                _members.Remove(member);
            else
                throw new CustomerException("Error while removing member - Member doesn't exist");
        }
    }
}
