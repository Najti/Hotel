using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Persistence.Repositories;
using Hotel.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        private ObservableCollection<Hotel.Domain.Model.Customer> customers = new ObservableCollection<Hotel.Domain.Model.Customer>();
        private CustomerManager customerManager;
        //private string conn = "Data Source=NB21-6CDPYD3\\SQLEXPRESS;Initial Catalog=HotelDonderdag;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
            customerManager = new CustomerManager(RepositoryFactory.CustomerRepository);
            customers = new ObservableCollection<Hotel.Domain.Model.Customer>(customerManager.GetCustomers(null));
            CustomerDataGrid.ItemsSource = customers;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            customers = new ObservableCollection<Hotel.Domain.Model.Customer>(customerManager.GetCustomers(SearchTextBox.Text));
            CustomerDataGrid.ItemsSource = customers;
        }

        private void MenuItemAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow w = new CustomerWindow(null);
            if (w.ShowDialog() == true)
                customers.Add(w.Customer);
        }
        private void MenuItemDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Hotel.Domain.Model.Customer c = new Hotel.Domain.Model.Customer();
                customerManager.DeleteCustomer((Hotel.Domain.Model.Customer)CustomerDataGrid.SelectedItem);
            }
            else
            {
                // User clicked No
                // Perform action for No ("y" in this case)
                Console.WriteLine("y");
            }
        }

        private void MenuItemUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerDataGrid.SelectedItem == null) MessageBox.Show("No customer selected", "update");
            else
            {
                CustomerWindow w = new CustomerWindow((Hotel.Domain.Model.Customer)CustomerDataGrid.SelectedItem);
                w.ShowDialog();
            }
        }
        
            private void MenuItemGetMembers_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerDataGrid.SelectedItem == null) MessageBox.Show("No customer selected", "update");
            else
            {
                CustomerWindow w = new CustomerWindow((Hotel.Domain.Model.Customer)CustomerDataGrid.SelectedItem);
                w.ShowDialog();
            }
        }

        private void ActivityButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerDataGrid.SelectedItem == null)
            {
                ViewActivitiesWindow vaw = new ViewActivitiesWindow();
                vaw.ShowDialog();
            }
            else
            {
                ViewActivitiesWindow vaw = new ViewActivitiesWindow((Hotel.Domain.Model.Customer)CustomerDataGrid.SelectedItem);
                vaw.ShowDialog();
            }
        }
    }
}
