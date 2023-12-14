using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Model
{
    public class Member
    {
        public Member(string name, DateTime birthday)
        {
            Name = name;
            Birthday = birthday;
        }

        public Member(int id, Customer customer, string name, DateTime birthdate)
        {
            _id = id;
            _customer = customer;
            _name = name;
            _birthday = birthdate;
        }

        private string _name;
        public string Name
        {
            get
            { return _name; }
            set
            { if (string.IsNullOrWhiteSpace(value)) throw new CustomerException("Member name cannot be empty."); _name = value; }
        }
        private DateTime _birthday;
        public DateTime Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                if (DateTime.Now <= value) throw new CustomerException("Member birthday cannot be now or in the future.");
                _birthday = value;
            }
        }
        private Customer _customer;
        public Customer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
            }
        }
        private int _id;
        public int Id
        {
            get { return _id; } set { _id = value; }
        }

    }
}