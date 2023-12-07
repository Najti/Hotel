using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Managers
{
    public class RegistrationManager
    {
        private IRegistrationRepository registrationRepository;
        public RegistrationManager(IRegistrationRepository registrationRepository)
        {
            this.registrationRepository = registrationRepository;
        }
        public void AddRegistration(Registration registration)
        {
            registrationRepository.AddRegistration(registration);
        }
    }
}
