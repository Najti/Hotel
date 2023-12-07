using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Managers
{
    public class MemberManager
    {
        private IMemberRepository memberRepository;
        public MemberManager(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }
        public void AddMember(Member member, int customerid)
        {
            memberRepository.AddMember(member, customerid);
        }
        public void UpdateMember(Member member, int customerid, string name)
        {
            memberRepository.UpdateMember(member, customerid, name);
        }
        public void DeleteMember(Member member, int customerid)
        {
            memberRepository.DeleteMember(member, customerid);
        }

        public List<Member> GetMembers(int customerid)
        {
            return memberRepository.GetMembers(customerid);
        }
    }
}
