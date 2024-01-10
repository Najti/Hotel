using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using Hotel.Persistence.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestProject.Repositories
{
    public class RegistrationRepoTest
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationRepoTest()
        {
            // Your test connection string
            string testConnectionString = "Your_Test_Connection_String";
            _registrationRepository = new RegistrationRepository(testConnectionString);
        }

        [Fact]
        public void AddRegistration_ValidRegistration_ShouldAddToDatabase()
        {
            // Arrange
            var mockActivity = new Mock<Activity>(); // Mocking the Activity class
            mockActivity.SetupGet(a => a.Id).Returns(1); // Example setup for Activity's ID
            // Set up other properties as needed for Activity

            var mockCustomer = new Mock<Customer>(); // Mocking the Customer class
            mockCustomer.SetupGet(c => c.Id).Returns(1); // Example setup for Customer's ID
            // Set up other properties as needed for Customer

            var registration = new Registration(mockCustomer.Object, mockActivity.Object)
            {
                NumberOfAdults = 2,
                NumberOfChildren = 1,
                Price = 100 // This should be calculated based on activity and number of adults/children
            };

            // Act
            _registrationRepository.AddRegistration(registration);

            // Assert - Here, you can verify if the registration was correctly added to the database
            // Retrieve the registration from the database and check if it matches what you added
            var retrievedRegistrations = _registrationRepository.GetRegistrationsByCustomer(mockCustomer.Object);
            Assert.Contains(retrievedRegistrations, r => r.Id == registration.Id);
        }

        [Fact]
        public void GetRegistrationsByCustomer_ValidCustomer_ShouldReturnRegistrations()
        {
            // Arrange
            var mockCustomer = new Mock<Customer>(); // Mocking the Customer class
            mockCustomer.SetupGet(c => c.Id).Returns(1); // Example setup for Customer's ID
            // Set up other properties as needed for Customer

            // Act
            var registrations = _registrationRepository.GetRegistrationsByCustomer(mockCustomer.Object);

            // Assert
            Assert.NotNull(registrations);
            // Perform further verifications on the retrieved registrations based on expected data
        }

        // You can write similar tests to handle exceptions and negative scenarios
    }
}
