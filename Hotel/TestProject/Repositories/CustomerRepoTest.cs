using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using Hotel.Persistence.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestProject.Repositories
{
    public class CustomerRepoTest
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerRepoTest()
        {
            // Setup: Gebruik een mock van de database of een in-memory database voor tests
            string testConnectionString = "Data Source=LAPTOP-TLTEA25D\\SQLEXPRESS;Initial Catalog=HotelTesting;Integrated Security=True";
            _customerRepository = new CustomerRepository(testConnectionString);
        }

        [Fact]
        public void GetCustomerByName_ExistingName_ShouldReturnCustomer()
        {
            // Arrange
            string existingName = "John Doe";

            // Act
            var customer = _customerRepository.GetCustomerByName(existingName);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(existingName, customer.Name);
        }

        [Fact]
        public void GetCustomerByID_ExistingID_ShouldReturnCustomer()
        {
            // Arrange
            int existingID = 1;

            // Act
            var customer = _customerRepository.GetCustomerByID(existingID);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(existingID, customer.Id);
        }

        [Fact]
        public void AddCustomer_ValidCustomer_ShouldAddToDatabase()
        {
            // Arrange
            var customer = new Customer("Jane Doe", new ContactInfo("jane@example.com", "987654321", new Address("New York", "Street", "54321", "1B")));

            // Act
            _customerRepository.AddCustomer(customer);

            // Assert
            var retrievedCustomer = _customerRepository.GetCustomerByName("Jane Doe");
            Assert.NotNull(retrievedCustomer);
            Assert.Equal(customer.Name, retrievedCustomer.Name);
            // You can perform additional assertions based on the data stored in the database
        }

        [Fact]
        public void UpdateCustomer_ValidCustomer_ShouldUpdateInDatabase()
        {
            // Arrange
            var customer = _customerRepository.GetCustomerByName("John Doe");
            customer.Contact.Email = "new_email@example.com";
            customer.Contact.Phone = "555555555";

            // Act
            _customerRepository.UpdateCustomer(customer);

            // Assert
            var updatedCustomer = _customerRepository.GetCustomerByID(customer.Id);
            Assert.NotNull(updatedCustomer);
            Assert.Equal(customer.Contact.Email, updatedCustomer.Contact.Email);
            Assert.Equal(customer.Contact.Phone, updatedCustomer.Contact.Phone);
        }

        [Fact]
        public void DeleteCustomer_ValidCustomer_ShouldRemoveFromDatabase()
        {
            // Arrange
            var customer = _customerRepository.GetCustomerByName("Jane Doe");

            // Act
            _customerRepository.DeleteCustomer(customer);

            // Assert
            var deletedCustomer = _customerRepository.GetCustomerByName("Jane Doe");
            Assert.Null(deletedCustomer);
        }

        // Write similar tests to handle exceptions that your repository may throw in various scenarios
    }
}
