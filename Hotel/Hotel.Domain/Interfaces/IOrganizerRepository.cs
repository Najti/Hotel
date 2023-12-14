using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces
{
    public interface IOrganizerRepository
    {
        List<Organizer> GetOrganizers(string filter);
        Organizer GetOrganizer(int id);
        void DeleteOrganizer(Organizer organizer);
        Organizer AddOrganizer(Organizer organizer);
        void UpdateOrganizer(Organizer organizer);
    }
}
