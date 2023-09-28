using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContactInfo Contact { get; set; }
        public List<Member> Members { get; set; } = new List<Member>(); //gn dubbels
        public void AddMember(Member member)
        {

        }
        public void RemoveMember(Member member) 
        { 
        }
    }
}
