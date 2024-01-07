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
        public Hotel.Domain.Model.Customer Customer { get; set; }
        private CustomerManager customerManager;
        public CustomerWindow(Hotel.Domain.Model.Customer customer)
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
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Customer == null)
            {


        //        < TextBox Grid.Row = "0" Grid.Column = "1" Name = "IdTextBox" Margin = "5" VerticalAlignment = "Center" />
        //< TextBox Grid.Row = "0" Grid.Column = "3" Name = "CityTextBox" Margin = "5" VerticalAlignment = "Center" />
        //< TextBox Grid.Row = "1" Grid.Column = "1" Name = "NameTextBox" Margin = "5" VerticalAlignment = "Center" />
        //< TextBox Grid.Row = "1" Grid.Column = "3" Name = "ZipTextBox" Margin = "5" VerticalAlignment = "Center" />
        //< TextBox Grid.Row = "2" Grid.Column = "1" Name = "EmailTextBox" Margin = "5" VerticalAlignment = "Center" />
        //< TextBox Grid.Row = "2" Grid.Column = "3" Name = "StreetTextBox" Margin = "5" VerticalAlignment = "Center" />
        //< TextBox Grid.Row = "3" Grid.Column = "1" Name = "PhoneTextBox" Margin = "5" VerticalAlignment = "Center" />
        //< TextBox Grid.Row = "3" Grid.Column = "3" Name = "HouseNumberTextBox" Margin = "5" VerticalAlignment = "Center" />

                //Nieuw
                //wegschrijven
                //TODO nrofmembers
                Address address = new Address(CityTextBox.Text, StreetTextBox.Text, ZipTextBox.Text, HouseNumberTextBox.Text);
                ContactInfo contactinfo = new ContactInfo(EmailTextBox.Text, PhoneTextBox.Text, address);
                Hotel.Domain.Model.Customer customer = new Hotel.Domain.Model.Customer(NameTextBox.Text, contactinfo);
                customerManager.AddCustomer(customer);
                Customer = customer;
            }
            else
            {
                //Update
                //update DB
                
                Customer.Contact.Email=EmailTextBox.Text;
                Customer.Contact.Phone=PhoneTextBox.Text;
                Customer.Name=NameTextBox.Text;
            }
            DialogResult = true;
            Close();
        }
    }
}
