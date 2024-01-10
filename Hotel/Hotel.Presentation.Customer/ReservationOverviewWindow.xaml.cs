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
    public partial class ReservationOverviewWindow : Window
    {
        private RegistrationManager registrationManager;

        public ReservationOverviewWindow(Hotel.Domain.Model.Customer customer)
        {
            try
            {
                InitializeComponent();

                registrationManager = new RegistrationManager(RepositoryFactory.RegistrationRepository);

                // Haal de registraties op
                var registrations = registrationManager.GetRegistrationsByCustomer(customer);

                // Wijs de DataContext toe aan de DataGrid
                dataGrid.ItemsSource = registrations;

                // Voeg de eventhandler toe voor MouseDoubleClick op de DataGrid
                dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ReservationOverviewWindow()
        {
            try
            {
                InitializeComponent();

                registrationManager = new RegistrationManager(RepositoryFactory.RegistrationRepository);

                // Haal de registraties op
                var registrations = registrationManager.GetRegistrationsByCustomer(null);

                // Wijs de DataContext toe aan de DataGrid
                dataGrid.ItemsSource = registrations;

                // Voeg de eventhandler toe voor MouseDoubleClick op de DataGrid
                dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem is Registration selectedRegistration)
                {
                    ReservationWindow reservationWindow = new ReservationWindow();
                    reservationWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }

}
