using Hotel.Domain.Exceptions;
using Hotel.Domain.Model;
using Hotel.Persistence.Repositories;
using System.Xml.Linq;

namespace ConsoleAppDL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string conn = "Data Source=LAPTOP-TLTEA25D\\SQLEXPRESS;Initial Catalog=HotelApplicatie;Integrated Security=True";
            ActivityRepository repo = new ActivityRepository(conn);
            CustomerRepository Crepo = new CustomerRepository(conn);
            MemberRepository mRepo = new MemberRepository(conn);
            OrganizerRepository oRepo = new OrganizerRepository(conn);

            foreach (Activity activity in repo.GetActivities(""))
            {
                Console.WriteLine(activity.Name);
            }
            foreach (Customer customer in Crepo.GetCustomers(""))
            {
                Console.WriteLine(customer.Name);
            }
            List<Member> members= mRepo.GetMembers("");
            foreach(Member member in members)
            {
                Console.WriteLine(member.Name);
            }
            List<Organizer> organizers = oRepo.GetOrganizers("");
            foreach (Organizer orgizer in organizers) { Console.WriteLine(orgizer.Name);}

        }
    }
}