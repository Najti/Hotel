using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces
{
    public interface IRegistrationRepository
    {
        void AddRegistration(Registration registration);
        public List<Registration> GetRegistrationsByCustomer(Customer customer);
    }
}
