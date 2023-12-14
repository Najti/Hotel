using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Managers
{
    public class CustomerManager
    {
        private ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetCustomers(string filter)
        {
            try
            {
                return _customerRepository.GetCustomers(filter);
            }
            catch(Exception ex)
            {
                throw new CustomerManagerException("GetCustomers");
            }
        }

        public void AddCustomer(Customer customer)
        {
            try
            {
                _customerRepository.AddCustomer(customer);
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("Error while adding customer", ex);
            }
        }

        public Customer GetCustomerByID(int id)
        {
            try
            {
                return _customerRepository.GetCustomerByID(id);
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("GetCustomerByID");
            }
        }
        public Customer UpdateCustomer(Customer customer)
        {
            try
            {
                return _customerRepository.UpdateCustomer(customer);
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("Update Customer");
            }
        }
        public void DeleteCustomer(Customer customer)
        {
            try
            {
                 _customerRepository.DeleteCustomer(customer);
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("DeleteCustomers");
            }
        }
    }
}
