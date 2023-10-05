using Hotel.Persistence.Repositories;

namespace ConsoleAppDL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string conn = "Data Source=NB21-6CDPYD3\\SQLEXPRESS;Initial Catalog=HotelDonderdag;Integrated Security=True";
            CustomerRepository repo = new CustomerRepository(conn);
            var x = repo.GetCustomers("ge");

        }
    }
}