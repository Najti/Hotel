using Hotel.Domain.Managers;
using Hotel.Persistence.Repositories;
using Hotel.Presentation.Customer.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel.Presentation.Customer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CustomerUI> customerUIs=new List<CustomerUI>();
        private CustomerManager customerManager;
        private string conn = "Data Source=NB21-6CDPYD3\\SQLEXPRESS;Initial Catalog=HotelDonderdag;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
            customerManager = new CustomerManager(new CustomerRepository(conn));
            customerUIs = customerManager.GetCustomers(null).Select(x => new CustomerUI(x.Id,x.Name,x.Contact.Email,x.Contact.Address.ToString(),x.Contact.Phone,x.GetMembers().Count)).ToList();
            CustomerDataGrid.ItemsSource = customerUIs;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            customerUIs = customerManager.GetCustomers(SearchTextBox.Text).Select(x => new CustomerUI(x.Id, x.Name, x.Contact.Email, x.Contact.Address.ToString(), x.Contact.Phone, x.GetMembers().Count)).ToList();
            CustomerDataGrid.ItemsSource = customerUIs;
        }

        private void MenuItemAddCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
