using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces
{
    public interface IMemberRepository
    {
        void AddMember(Member member, int customerid);
        void UpdateMember(Member member, int customerid, string name);
        void DeleteMember(Member member, int customerid);
        List<Member> GetMembers(int customerid);
    }
}
