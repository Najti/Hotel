using Hotel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Model
{
    public class Activity
    {
        public Activity(int id, string name, string description, DateTime date, int duration, int availablePlaces, decimal priceAdult, decimal priceChild, decimal discount, string location)
        {
            _id = id;
            _name = name;
            _description = description;
            _date = date;
            _duration = duration;
            _availablePlaces = availablePlaces;
            _priceAdult = priceAdult;
            _priceChild = priceChild;
            _discount = discount;
            _location = location;
        }

        public int Id
        {
            get { return _id; }
            set
            {
                if (value <= 0)
                {
                    throw new ActivityException("ID cannot be less than 1");
                }
                _id = value;
            }
        }
        private int _id;

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ActivityException("Name cannot be empty or spaces");
                }
                _name = value;
            }
        }
        private string _name;

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ActivityException("Description cannot be empty or spaces");
                }
                _description = value;
            }
        }
        private string _description;

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ActivityException("Date cannot be default value");
                }
                _date = value;
            }
        }

        private DateTime _date;

        public int Duration
        {
            get { return _duration; }
            set
            {
                if (value <= 0)
                {
                    throw new ActivityException("Duration cannot be less than or equal to 0");
                }
                _duration = value;
            }
        }
        private int _duration;

        public int AvailablePlaces
        {
            get { return _availablePlaces; }
            set
            {
                if (value <= 0)
                {
                    throw new ActivityException("AvailablePlaces cannot be less than or equal to 0");
                }
                _availablePlaces = value;
            }
        }
        private int _availablePlaces;

        public decimal PriceAdult
        {
            get { return _priceAdult; }
            set
            {
                if (value <= 0)
                {
                    throw new ActivityException("PriceAdult cannot be less than or equal to 0");
                }
                _priceAdult = value;
            }
        }
        private decimal _priceAdult;

        public decimal PriceChild
        {
            get { return _priceChild; }
            set
            {
                if (value <= 0)
                {
                    throw new ActivityException("PriceChild cannot be less than or equal to 0");
                }
                _priceChild = value;
            }
        }
        private decimal _priceChild;

        public string Location
        {
            get { return _location; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ActivityException("Location cannot be empty or spaces");
                }
                _location = value;
            }
        }
        private string _location;

        public decimal? Discount
        {
            get { return _discount; }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ActivityException("Discount must be between 0 and 100%");
                }
                _discount = value;
            }
        }
        private decimal? _discount;



    }
}
