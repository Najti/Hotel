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
        void AddMember(Member member);
        void UpdateMember(Member member);
        void DeleteMember(Member member);
        List<Member> GetMembers(string filter); 
        Member GetMemberByID(int customerid);
    }
}
