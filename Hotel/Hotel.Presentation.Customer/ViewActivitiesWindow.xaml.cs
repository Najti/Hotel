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
    /// Interaction logic for ViewActivitiesWindow.xaml
    /// </summary>
    public partial class ViewActivitiesWindow : Window
    {
        private ActivityManager activityManager;
        private RegistrationManager registrationManager;
        private Domain.Model.Customer selectedCustomer;
        public event EventHandler<Activity> ActivitySelected;


        public ViewActivitiesWindow()
        {
            InitializeComponent();
            activityManager = new ActivityManager(RepositoryFactory.ActivityRepository);
            ; RefreshActivities();
        }

        public ViewActivitiesWindow(Domain.Model.Customer selectedCustomer)
        {
            this.selectedCustomer = selectedCustomer;
            InitializeComponent();
            activityManager = new ActivityManager(RepositoryFactory.ActivityRepository);
            registrationManager = new RegistrationManager(RepositoryFactory.RegistrationRepository);
            ; RefreshActivitiesForUser();
        }

        private void SearchButton_Click(object sender, object e)
        {
            try
            {
                ActivitiesDataGrid.ItemsSource = activityManager.GetActivities(SearchTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }          
        }

        private void RefreshActivities()
        {
            try
            {
                //show all activities
                ActivitiesDataGrid.ItemsSource = activityManager.GetActivities(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshActivitiesForUser()
        {
            try
            {
                //show all activities
                // ActivitiesDataGrid.ItemsSource = registrationManager.GetRegistrationsByCustomer(selectedCustomer);

                //if (selectedCustomer != null) { } else { ActivitiesDataGrid.ItemsSource = activityManager.GetActivities(null); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshActivities();
        }

        private void ActivitiesDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ActivitiesDataGrid.SelectedItem is Activity selectedActivity)
            {
                ActivitySelected?.Invoke(this, selectedActivity);
                Close();
            }
        }
    }
}
