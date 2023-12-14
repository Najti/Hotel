using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Presentation.Customer.Model;
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
        public CustomerUI CustomerUI { get; set; }
        private CustomerManager customerManager;
        public CustomerWindow(CustomerUI customerUI)
        {
            InitializeComponent();
            customerManager = new CustomerManager(RepositoryFactory.CustomerRepository);
            this.CustomerUI = customerUI;
            if (CustomerUI != null)
            {
                IdTextBox.Text = CustomerUI.Id.ToString();
                NameTextBox.Text = CustomerUI.Name;
                EmailTextBox.Text = CustomerUI.Email;
                PhoneTextBox.Text = CustomerUI.Phone;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerUI == null)
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
                CustomerUI = new CustomerUI(NameTextBox.Text, EmailTextBox.Text, address.ToString(), PhoneTextBox.Text, 0);
            }
            else
            {
                //Update
                //update DB
                
                CustomerUI.Email=EmailTextBox.Text;
                CustomerUI.Phone=PhoneTextBox.Text;
                CustomerUI.Name=NameTextBox.Text;
            }
            DialogResult = true;
            Close();
        }
    }
}
