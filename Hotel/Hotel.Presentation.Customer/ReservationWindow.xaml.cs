using Hotel.Domain.Managers;
using Hotel.Domain.Model;
using Hotel.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hotel.Presentation.Customer
{
    /// <summary>
    /// Interaction logic for ReservationWindow.xaml
    /// </summary>
    public partial class ReservationWindow : Window
    {
        private CustomerManager customerManager;
        private ActivityManager activityManager;
        private RegistrationManager rm;
        private Hotel.Domain.Model.Customer loggedInCustomer;
        private Activity selectedActivity;
        private List<Member> selectedMembers = new List<Member>(); // Initialiseer de lijst

        public ReservationWindow()
        {
            try
            {
                InitializeComponent();

                // Initialiseer managers
                customerManager = new CustomerManager(RepositoryFactory.CustomerRepository);
                activityManager = new ActivityManager(RepositoryFactory.ActivityRepository);
                rm = new RegistrationManager(RepositoryFactory.RegistrationRepository);

                // Verberg delen die afhankelijk zijn van inlogstatus
                WelcomeLabel.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Collapsed;
                SelectActivityButton.Visibility = Visibility.Collapsed;

                // Andere delen van UI in eerste instantie verbergen
                SelectedActivityLabel.Visibility = Visibility.Collapsed;
                ActivityComboBox.Visibility = Visibility.Collapsed;
                SelectMembersLabel.Visibility = Visibility.Collapsed;
                MembersListBox.Visibility = Visibility.Collapsed;
                //TotalPriceAdultLabel.Visibility = Visibility.Collapsed;
                //TotalPriceChildLabel.Visibility = Visibility.Collapsed;
                TotalPriceLabel.Visibility = Visibility.Collapsed;

                // Event handlers toevoegen
                SelectActivityButton.Click += SelectActivityButton_Click;
                LogoutButton.Click += LogoutButton_Click;

                // Andere initialisatie logica die nodig is voor je applicatie
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectActivityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewActivitiesWindow vaw = new ViewActivitiesWindow();
                vaw.ActivitySelected += ViewActivitiesWindow_ActivitySelected;
                vaw.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewActivitiesWindow_ActivitySelected(object sender, Activity selectedActivity)
        {
            try
            {
                this.selectedActivity = selectedActivity;
                SelectedActivityLabel.Content = selectedActivity.Name;
                SelectedActivityLabel.Visibility = Visibility.Visible;
                MembersListBox.ItemsSource = loggedInCustomer.GetMembers();
                MembersListBox.DisplayMemberPath = "Name";
                CalculatePrice();
                PricePanel.Visibility = Visibility.Visible;
                TotalPriceLabel.Visibility = Visibility.Visible;
                SelectMembersPanel.Visibility = Visibility.Visible;
                SelectMembersLabel.Visibility = Visibility.Visible;
                MembersListBox.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Handel uitloggen af door UI-elementen aan te passen
                loggedInCustomer = null;
                LoginLabel.Visibility = Visibility.Visible;
                WelcomeLabel.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Collapsed;
                EmailTextBox.Visibility = Visibility.Visible;
                EmailTextBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculatePrice()
        {
            try
            {
                decimal totalPrice = selectedActivity.PriceAdult; // Start with the price for the customer
                if (selectedMembers != null)
                {
                    foreach (Member m in selectedMembers)
                    {
                        // Bereken de leeftijd van de leden
                        int age = DateTime.Today.Year - m.Birthday.Year;

                        // Voeg de prijs toe op basis van leeftijd
                        if (age >= 18)
                        {
                            totalPrice += selectedActivity.PriceAdult; // Prijs voor volwassenen
                        }
                        else
                        {
                            totalPrice += selectedActivity.PriceChild; // Prijs voor kinderen
                        }
                    }
                }

                // Doe iets met de totale prijs, bijv. toon het in een label op het scherm
                TotalPriceLabel.Content = $"Total Price: {totalPrice:C}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Andere logica voor het selecteren van activiteiten en leden, en het berekenen van prijzen, kan hier worden toegevoegd

        // Voorbeeld van het weergeven van geselecteerde activiteit en leden in de UI
        private void DisplaySelectedActivityAndMembers()
        {
            try
            {
                if (selectedActivity != null)
                {
                    SelectedActivityLabel.Content = $"Selected Activity: {selectedActivity.Name}";
                    SelectedActivityLabel.Visibility = Visibility.Visible;
                }

                if (selectedMembers != null && selectedMembers.Any())
                {
                    // Weergave van geselecteerde leden in ListBox of andere UI-elementen
                    MembersListBox.ItemsSource = selectedMembers;
                    SelectMembersLabel.Visibility = Visibility.Visible;
                    MembersListBox.Visibility = Visibility.Visible;

                    // Mogelijkheid om prijzen te berekenen en weer te geven op basis van geselecteerde leden en activiteit
                    // TotalPriceAdultLabel.Visibility = Visibility.Visible;
                    // TotalPriceChildLabel.Visibility = Visibility.Visible;
                    // TotalPriceLabel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            loggedInCustomer = customerManager.GetCustomerByName(EmailTextBox.Text);
            if (loggedInCustomer != null)
            {
                // Pas UI aan voor ingelogde klant
                EmailTextBox.Visibility = Visibility.Collapsed;
                LogInButton.Visibility = Visibility.Collapsed;
                LoginLabel.Visibility = Visibility.Collapsed;
                WelcomeLabel.Content = $"Welcome {loggedInCustomer.Name}!";
                WelcomeLabel.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Visible;
                ActivityPanel.Visibility = Visibility.Visible;
                SelectActivityButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("User bestaat niet");
            }
        }

        private void MembersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var selectedMember in e.AddedItems)
            {
                selectedMembers.Add((Member)selectedMember);
            }

            foreach (var unselectedMember in e.RemovedItems)
            {
                selectedMembers.Remove((Member)unselectedMember);
            }

            CalculatePrice();
        }

        private void MakeReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hotel.Domain.Model.Customer c = loggedInCustomer;

                // Remove all existing members
                foreach (Member m in c.GetMembers().ToList()) // ToList() creates a copy to avoid modification during enumeration
                {
                    c.RemoveMember(m);
                }

                // Add selected members
                foreach (Member m in selectedMembers)
                {
                    c.AddMember(m);
                }

                // Attempt to create registration
                Registration r = new Registration(c, selectedActivity);
                rm.AddRegistration(r);

                // Show success message if no exception occurred
                MessageBox.Show("Reservation made successfully");
            }
            catch (Exception ex)
            {
                // Show error message if an exception occurred
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            // Verberg alle relevante elementen
            SelectedActivityLabel.Visibility = Visibility.Collapsed;
            MembersListBox.Visibility = Visibility.Collapsed;
            SelectMembersLabel.Visibility = Visibility.Collapsed;
            PricePanel.Visibility = Visibility.Collapsed;
            TotalPriceLabel.Visibility = Visibility.Collapsed;

            // Laat alleen de welkomsttekst en de knop 'Select Activity' zien
            WelcomeLabel.Visibility = Visibility.Visible;
            SelectActivityButton.Visibility = Visibility.Visible;
        }

    }
}
