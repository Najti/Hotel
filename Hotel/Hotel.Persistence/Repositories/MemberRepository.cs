using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Persistence.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public void AddMember(Member member, int customerid)
        {
            throw new NotImplementedException();
        }

        public void DeleteMember(Member member, int customerid)
        {
            throw new NotImplementedException();
        }

        public List<Member> GetMembers(int customerid)
        {
            throw new NotImplementedException();
        }

        public void UpdateMember(Member member, int customerid, string name)
        {
            throw new NotImplementedException();
        }
    }
}
