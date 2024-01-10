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
        private List<Organizer> organizers = new List<Organizer>();
        private CustomerManager customerManager;
        private OrganizerManager om;
        //private string conn = "Data Source=NB21-6CDPYD3\\SQLEXPRESS;Initial Catalog=HotelDonderdag;Integrated Security=True";
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                om = new OrganizerManager(RepositoryFactory.OrganizerRepository);
                customerManager = new CustomerManager(RepositoryFactory.CustomerRepository);
                customers = new ObservableCollection<Hotel.Domain.Model.Customer>(customerManager.GetCustomers(null));
                CustomerDataGrid.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrganizerButton.Content.ToString() == "See Organizers")
                {
                    customers = new ObservableCollection<Hotel.Domain.Model.Customer>(customerManager.GetCustomers(SearchTextBox.Text));
                    CustomerDataGrid.ItemsSource = customers;
                }
                else
                {
                    organizers = new List<Organizer>(om.GetOrganizers(SearchTextBox.Text));
                    CustomerDataGrid.ItemsSource = organizers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrganizerButton.Content.ToString() == "See Organizers")
                {
                    CustomerWindow w = new CustomerWindow((Domain.Model.Customer)null);
                    if (w.ShowDialog() == true)
                        MessageBox.Show("Added customer successfully");
                }
                else
                {
                    CustomerWindow w = new CustomerWindow((Organizer)null);
                    if (w.ShowDialog() == true)
                        MessageBox.Show("Added organizer successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (CustomerDataGrid.SelectedItem is Hotel.Domain.Model.Customer customer)
                    {
                        customerManager.DeleteCustomer(customer);
                        MessageBox.Show("Customer deleted successfully");
                    }
                    else if (CustomerDataGrid.SelectedItem is Organizer organizer)
                    {
                        // Logic to delete organizer
                        // organizerManager.DeleteOrganizer(organizer);
                        MessageBox.Show("Organizer deleted successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CustomerDataGrid.SelectedItem is Hotel.Domain.Model.Customer selectedCustomer)
                {
                    if (selectedCustomer == null)
                    {
                        MessageBox.Show("No customer selected", "Update");
                    }
                    else
                    {
                        CustomerWindow w = new CustomerWindow(selectedCustomer);
                        w.ShowDialog();
                    }
                }
                else if (CustomerDataGrid.SelectedItem is Organizer selectedOrganizer)
                {
                    CustomerWindow w = new CustomerWindow(selectedOrganizer);
                    w.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No organizer selected", "Update");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //private void MenuItemGetMembers_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CustomerDataGrid.SelectedItem == null) MessageBox.Show("No customer selected", "update");
        //    else
        //    {
        //        CustomerWindow w = new CustomerWindow((Hotel.Domain.Model.Customer)CustomerDataGrid.SelectedItem);
        //        w.ShowDialog();
        //    }
        //}

        private void OrganizerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var columnToHide = CustomerDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "NrOfMembers") as DataGridColumn;
                if (OrganizerButton.Content.ToString() != "See Customers")
                {
                    organizers = new List<Organizer>(om.GetOrganizers(SearchTextBox.Text));
                    CustomerDataGrid.ItemsSource = organizers;
                    OrganizerButton.Content = "See Customers";
                    columnToHide.Visibility = Visibility.Collapsed;
                    NewActivityButton.IsEnabled = true;
                    ReservationButton.IsEnabled = false;
                    NewReservationButton.IsEnabled = false;
                }
                else
                {
                    CustomerDataGrid.ItemsSource = customers;
                    OrganizerButton.Content = "See Organizers";
                    NewActivityButton.IsEnabled = false;
                    ReservationButton.IsEnabled = true;
                    NewReservationButton.IsEnabled = true;
                    columnToHide.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReservationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CustomerDataGrid.SelectedItem == null)
                {
                    ReservationOverviewWindow row = new ReservationOverviewWindow();
                    row.ShowDialog();
                }
                else
                {
                    ReservationOverviewWindow row = new ReservationOverviewWindow((Hotel.Domain.Model.Customer)CustomerDataGrid.SelectedItem);
                    row.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActivityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewActivitiesWindow vaw = new ViewActivitiesWindow();
                vaw.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeselectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerDataGrid.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewActivityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CustomerDataGrid.SelectedItem == null) MessageBox.Show("No organizer selected", "Update");
                else
                {
                    AddNewActivityWindow anaw = new AddNewActivityWindow((Organizer)CustomerDataGrid.SelectedItem);
                    anaw.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewReservationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReservationWindow rw = new ReservationWindow();
                rw.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
