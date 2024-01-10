using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        List<Customer> GetCustomers(string filter);
        Customer GetCustomerByID(int id);
        Customer UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Customer GetCustomerByName(string name);
    }
}
