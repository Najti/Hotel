using Hotel.Domain.Exceptions;
using Hotel.Domain.Model;
using Xunit;

namespace TestProject.Models
{
    public class ReservationTests
    {
        [Fact]
        public void CreateRegistration_ValidCustomerAndActivity_ShouldCreateRegistration()
        {
            // Arrange
            Customer customer = new Customer("John Doe", new ContactInfo("john@example.com", "123456789", new Address("New York", "Street", "12345", "1A")));
            Activity activity = new Activity(1, "Event", "Description", DateTime.Now, 60, 100, 50, 25, 10, "Location");

            // Act
            var registration = new Registration(customer, activity);

            // Assert
            Assert.NotNull(registration);
            Assert.Equal(customer, registration.Customer);
            Assert.Equal(activity, registration.Activity);
        }

        [Fact]
        public void CalculatePrice_ActivityWithDiscount_ShouldCalculateCorrectPrice()
        {
            // Arrange
            Customer customer = new Customer("John Doe", new ContactInfo("john@example.com", "123456789", new Address("New York", "Street", "12345", "1A")));
            Activity activity = new Activity(1, "Event", "Description", DateTime.Now, 60, 100, 50, 25, 10, "Location");
            Registration registration = new Registration(customer, activity);

            // Act
            registration.CalculatePrice();

            // Assert
            Assert.Equal(75, registration.Price); // Assuming a discount of 10% applies
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void SetInvalidRegistrationId_ShouldThrowRegistrationException(int invalidId)
        {
            // Arrange
            Customer customer = new Customer("John Doe", new ContactInfo("john@example.com", "123456789", new Address("New York", "Street", "12345", "1A")));
            Activity activity = new Activity(1, "Event", "Description", DateTime.Now, 60, 100, 50, 25, 10, "Location");
            Registration registration = new Registration(customer, activity);

            // Assert
            Assert.Throws<RegistrationException>(() => registration.Id = invalidId);
        }
    }
}
