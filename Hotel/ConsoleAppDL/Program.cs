using Hotel.Domain.Model;
using Hotel.Persistence.Repositories;

namespace ConsoleAppDL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string conn = "Data Source=LAPTOP-TLTEA25D\\SQLEXPRESS;Initial Catalog=HotelApplicatie;Integrated Security=True";
            ActivityRepository repo = new ActivityRepository(conn);
            //var x = repo.GetCustomers("ge");
            //Customer c=new Customer("piet",new ContactInfo("piet@yahoo","013456",new Address("Gent","Kerkstraat","9000","185")));
            //c.AddMember(new Member("paul", new DateOnly(2000, 5, 8)));
            //c.AddMember(new Member("rudy",new DateOnly(1987,1,1)));
            //repo.AddCustomer(c);
            //Console.WriteLine(repo.GetCustomers(""));
            foreach (Activity activity in repo.GetActivities("")) {
                Console.WriteLine(activity.Name);
            }
            
        }
    }
}