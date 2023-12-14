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
        public void AddMember(Member member)
        {
            memberRepository.AddMember(member);
        }
        public void UpdateMember(Member member)
        {
            memberRepository.UpdateMember(member);
        }
        public void DeleteMember(Member member)
        {
            memberRepository.DeleteMember(member);
        }

        public List<Member> GetMembers(string filter)
        {
            return memberRepository.GetMembers(filter);
        }
    }
}
