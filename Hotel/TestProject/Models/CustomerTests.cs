using Hotel.Domain.Exceptions;
using Hotel.Domain.Model;
using Xunit;

namespace TestProject.Models
{
    public class CustomerTests
    {
        [Fact]
        public void CreateCustomer_ValidName_ShouldCreateCustomer()
        {
            // Arrange
            string validName = "John Doe";
            ContactInfo contactInfo = new ContactInfo("john@example.com", "123456789", new Address("New York", "Street", "12345", "1A"));

            // Act
            var customer = new Customer(validName, contactInfo);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(validName, customer.Name);
            Assert.Equal(contactInfo, customer.Contact);
        }

        [Fact]
        public void AddMember_NewMember_ShouldIncreaseMemberCount()
        {
            // Arrange
            Customer customer = new Customer();
            Member newMember = new Member("Alice", DateTime.Now.AddYears(-25));

            // Act
            customer.AddMember(newMember);

            // Assert
            Assert.Single(customer.GetMembers());
        }

        [Fact]
        public void AddDuplicateMember_ShouldThrowCustomerException()
        {
            // Arrange
            Customer customer = new Customer();
            Member newMember = new Member("Alice", DateTime.Now.AddYears(-25));

            // Act
            customer.AddMember(newMember);

            // Assert
            Assert.Throws<CustomerException>(() => customer.AddMember(newMember));
        }

        [Fact]
        public void RemoveNonExistingMember_ShouldThrowCustomerException()
        {
            // Arrange
            Customer customer = new Customer();
            Member nonExistingMember = new Member("Bob", DateTime.Now.AddYears(-30));

            // Assert
            Assert.Throws<CustomerException>(() => customer.RemoveMember(nonExistingMember));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateCustomer_InvalidName_ShouldThrowCustomerException(string invalidName)
        {
            // Arrange
            ContactInfo contactInfo = new ContactInfo("john@example.com", "123456789", new Address("New York", "Street", "12345", "1A"));

            // Assert
            Assert.Throws<CustomerException>(() => new Customer(invalidName, contactInfo));
        }
    }
}
