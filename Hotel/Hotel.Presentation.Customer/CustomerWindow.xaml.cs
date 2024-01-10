using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hotel.Presentation.Customer
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public Hotel.Domain.Model.Customer Customer;
        public Organizer organizer;
        private CustomerManager customerManager;
        private OrganizerManager om;
        public CustomerWindow(Hotel.Domain.Model.Customer customer)
        {
            try
            {
                InitializeComponent();
                customerManager = new CustomerManager(RepositoryFactory.CustomerRepository);
                this.Customer = customer;
                if (Customer != null)
                {
                    IdTextBox.Text = Customer.Id.ToString();
                    NameTextBox.Text = Customer.Name;
                    EmailTextBox.Text = Customer.Contact.Email;
                    PhoneTextBox.Text = Customer.Contact.Phone;
                    CityTextBox.Text = Customer.Contact.Address.City;
                    ZipTextBox.Text = Customer.Contact.Address.PostalCode;
                    StreetTextBox.Text = Customer.Contact.Address.Street;
                    HouseNumberTextBox.Text = Customer.Contact.Address.HouseNumber;
                    AddButton.Content = "Update Customer";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public CustomerWindow(Organizer organizer)
        {
            try
            {
                InitializeComponent();
                om = new OrganizerManager(RepositoryFactory.OrganizerRepository);
                this.organizer = organizer;
                if (organizer != null)
                {
                    IdTextBox.Text = organizer.Id.ToString();
                    NameTextBox.Text = organizer.Name;
                    EmailTextBox.Text = organizer.Contact.Email;
                    PhoneTextBox.Text = organizer.Contact.Phone;
                    CityTextBox.Text = organizer.Contact.Address.City;
                    ZipTextBox.Text = organizer.Contact.Address.PostalCode;
                    StreetTextBox.Text = organizer.Contact.Address.Street;
                    HouseNumberTextBox.Text = organizer.Contact.Address.HouseNumber;
                    AddButton.Content = "Update organizer";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (om == null)
                {
                    if (Customer == null)
                    {
                        Address address = new Address(CityTextBox.Text, StreetTextBox.Text, ZipTextBox.Text, HouseNumberTextBox.Text);
                        ContactInfo contactinfo = new ContactInfo(EmailTextBox.Text, PhoneTextBox.Text, address);
                        Hotel.Domain.Model.Customer customer = new Hotel.Domain.Model.Customer(NameTextBox.Text, contactinfo);
                        customerManager.AddCustomer(customer);
                        Customer = customer;
                    }
                    else
                    {
                        Customer.Contact.Email = EmailTextBox.Text;
                        Customer.Contact.Phone = PhoneTextBox.Text;
                        Customer.Name = NameTextBox.Text;
                        Customer.Contact.Address.City = CityTextBox.Text;
                        Customer.Contact.Address.PostalCode = ZipTextBox.Text;
                        Customer.Contact.Address.Street = StreetTextBox.Text;
                        Customer.Contact.Address.HouseNumber = HouseNumberTextBox.Text;
                        customerManager.UpdateCustomer(Customer);
                    }
                }
                else
                {
                    if (organizer == null)
                    {
                        Address address = new Address(CityTextBox.Text, StreetTextBox.Text, ZipTextBox.Text, HouseNumberTextBox.Text);
                        ContactInfo contactinfo = new ContactInfo(EmailTextBox.Text, PhoneTextBox.Text, address);
                        Organizer organizerToAdd = new Organizer(NameTextBox.Text, contactinfo);
                        om.AddOrganizer(organizerToAdd);
                        organizer = organizerToAdd;
                    }
                    else
                    {
                        organizer.Contact.Email = EmailTextBox.Text;
                        organizer.Contact.Phone = PhoneTextBox.Text;
                        organizer.Name = NameTextBox.Text;
                        organizer.Contact.Address.City = CityTextBox.Text;
                        organizer.Contact.Address.PostalCode = ZipTextBox.Text;
                        organizer.Contact.Address.Street = StreetTextBox.Text;
                        organizer.Contact.Address.HouseNumber = HouseNumberTextBox.Text;
                        om.UpdateOrganizer(organizer);
                    }
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
