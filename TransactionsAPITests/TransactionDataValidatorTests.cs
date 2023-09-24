using Microsoft.AspNetCore.Http;
using System.Net;
using TransactionsAPI.Models.DTO;
using TransactionsAPI.Models.Validators;
using Xunit;

namespace TransactionsAPITests
{
    public class TransactionDataValidatorTests
    {
        private readonly TransactionDataValidator _validator;
        private readonly TransactionDTO _validTransactionDTO;

        public TransactionDataValidatorTests()
        {
            _validator = new TransactionDataValidator();
            _validTransactionDTO = new TransactionDTO()
            {
                Id = "ff289918-5ab7-4133-8f75-ab6cc508497a",
                ApplicationName = "Viva",
                Email = "lcubbit0@walmart.com",
                Filename = "Nonummy.png",
                Url = "http://vkontakte.ru",
                Inception = "8/2/2017",
                Amount = "$661.65",
                Allocation = "26.02"
            };
        }

        [Fact]
        public void Validate_ReturnsFalseWhenTransactionDTOIsNull()
        {
            // Act
            var result = _validator.Validate(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenTransactionDTOIsEmpty()
        {
            // Arrange
            var transactionData = new TransactionDTO()
            {
                Id = string.Empty, ApplicationName = string.Empty, Email = string.Empty, Filename = string.Empty,
                Url = string.Empty, Inception = string.Empty, Amount = string.Empty, Allocation = string.Empty
            };

            // Act
            var result = _validator.Validate(transactionData);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsTrueWhenValidTransactionDTO()
        {
            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenIdIsMissing()
        {
            // Arrange
            _validTransactionDTO.Id = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenIdNotGuid()
        {
            // Arrange
            _validTransactionDTO.Id = "Random string";

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenApplicationNameIsMissing()
        {
            // Arrange
            _validTransactionDTO.ApplicationName = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenApplicationNameExceedsMax()
        {
            // Arrange
            _validTransactionDTO.ApplicationName = new string('A', 201);

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenEmailIsMissing()
        {
            // Arrange
            _validTransactionDTO.Email = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenEmailExceedsMax()
        {
            // Arrange
            _validTransactionDTO.Email = new string('A', 201);

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenEmailNotValid()
        {
            // Arrange
            _validTransactionDTO.Email = "Random";

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsTrueWhenFileNameIsMissing()
        {
            // Arrange
            _validTransactionDTO.Filename = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenFileNameHasIncorrectFormat()
        {
            // Arrange
            _validTransactionDTO.Filename = "file.cs";

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsTrueWhenUrlIsMissing()
        {
            // Arrange
            _validTransactionDTO.Url = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenUrlIsIncorrect()
        {
            // Arrange
            _validTransactionDTO.Url = "Random";

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenInceptionIsMissing()
        {
            // Arrange
            _validTransactionDTO.Inception = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenInceptionIsNotInThePast()
        {
            // Arrange
            _validTransactionDTO.Inception = DateTime.Now.AddDays(1).ToString("M/d/yyyy");

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenAmountIsMissing()
        {
            // Arrange
            _validTransactionDTO.Amount = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenAmountDoesNotStartWithDollarOrEuroCurrency()
        {
            // Arrange
            _validTransactionDTO.Amount = "554.32";

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_ReturnsTrueWhenAllocationIsMissing()
        {
            // Arrange
            _validTransactionDTO.Allocation = null;

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_ReturnsFalseWhenAllocationExceedsLimits()
        {
            // Arrange
            _validTransactionDTO.Allocation = "101";

            // Act
            var result = _validator.Validate(_validTransactionDTO);

            // Assert
            Assert.False(result);
        }
    }
}
