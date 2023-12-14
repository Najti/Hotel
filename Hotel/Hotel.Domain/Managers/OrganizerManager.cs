using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Managers
{
    public class OrganizerManager
    {
        private IOrganizerRepository _organizerRepository;

        public OrganizerManager(IOrganizerRepository organizerRepository)
        {
            _organizerRepository = organizerRepository;
        }

        public List<Organizer> GetOrganizers(string filter)
        {
            return _organizerRepository.GetOrganizers(filter);
        } 
        public Organizer GetOrganizer(int id)
        {
            return _organizerRepository.GetOrganizer(id);
        }
        public void DeleteOrganizer(Organizer organizer)
        {
            _organizerRepository.DeleteOrganizer(organizer);
        }
        public Organizer AddOrganizer(Organizer organizer)
        {
            return _organizerRepository.AddOrganizer(organizer);
        }
        public void UpdateOrganizer(Organizer organizer)
        {
            _organizerRepository.UpdateOrganizer(organizer);
        }
    }
}
