using Hotel.Domain.Interfaces;
using Hotel.Persistence.Repositories;
using System.Configuration;

namespace Hotel.Util
{
    public static class RepositoryFactory
    {
        public static ICustomerRepository CustomerRepository { get { return new CustomerRepository(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString); } }
        public static IActivityRepository ActivityRepository { get { return new ActivityRepository(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString); } }
        public static IMemberRepository MemberRepository { get { return new MemberRepository(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString); } }
        public static IOrganizerRepository OrganizerRepository { get { return new OrganizerRepository(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString); } }
        public static IRegistrationRepository RegistrationRepository { get { return new RegistrationRepository(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString); } }
    }
}