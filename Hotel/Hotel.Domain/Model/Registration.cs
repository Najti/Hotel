using Hotel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Model
{
    public class Registration
    {
        public Registration(Customer customer, Activity activity)
        {
            _customer = customer;
            _activity = activity;
            _numberOfAdults = 1;
            AdultOrChild(customer);
            CalculatePrice();
        }
        public int Id
        {
            get
            { return _id; }
            set { if (value <= 0) throw new RegistrationException("Registration ID cannot be less than 1."); _id = value; }
        }
        private int _id;
        public Customer Customer
        {
            get
            { return _customer; }
            set
            { if (value == null) throw new RegistrationException("Customer cannot be empty."); _customer = value; }
        }
        private Customer _customer;
        public Activity Activity
        {
            get
            { return _activity; }
            set
            { if (value == null) throw new RegistrationException("Activity cannot be empty."); _activity = value; }
        }
        private Activity _activity;
        public decimal Price
        {
            get
            { return _price; }
            set
            { if (value < 0) throw new RegistrationException("Price cannot be less than 0."); _price = value; }
        }
        private decimal _price;
        public decimal costChild
        {
            get
            { return _costChild; }
            set
            { if (value < 0) throw new RegistrationException("Child cost cannot be less than 0."); _costChild = value; }
        }
        private decimal _costChild;
        public decimal costAdult
        {
            get
            { return _costAdult; }
            set
            { if (value < 0) throw new RegistrationException("Adult cost cannot be less than 0."); _costAdult = value; }
        }
        private decimal _costAdult;

        public int NumberOfAdults
        {
            get
            { return _numberOfAdults; }
            set
            { if (value <= 0) throw new RegistrationException("Number of adults cannot be less than 1."); _numberOfAdults = value; }
        }
        private int _numberOfAdults;
        public int NumberOfChildren
        {
            get
            { return _numberOfChildren; }
            set
            { if (value < 0) throw new RegistrationException("Number of children cannot be less than 0."); _numberOfChildren = value; }
        }
        private int _numberOfChildren;

        public void CalculatePrice()
        {
            
            if (_activity.Discount != 0)
            {
                costAdult = _activity.PriceAdult * (1 - (_activity.Discount ?? 0) / 100) * _numberOfAdults;
                if (Customer.GetMembers().Count != 0) costChild = _activity.PriceChild * (1 - (_activity.Discount ?? 0) / 100) * _numberOfChildren;
                else costChild = 0;
            }
            else
            {
                costAdult = _activity.PriceAdult * _numberOfAdults;
                if (Customer.GetMembers().Count != 0) costChild = _activity.PriceChild * _numberOfChildren;
                else costChild = 0;
            }
            Price = costAdult + costChild;
        }

        private void AdultOrChild(Customer customer)
        {
            foreach (Member member in customer.GetMembers())
            {
                DateTime dateTime = new DateTime(member.Birthday.Year, member.Birthday.Month, member.Birthday.Day);
                if (dateTime.AddYears(18) < DateTime.Now)
                {
                    _numberOfAdults++;
                }
                else
                {
                    _numberOfChildren++;
                }
            }
        }
    }
}

