using TransactionsAPI.Models;
using TransactionsAPI.Models.DTO;
using Xunit;

namespace TransactionsAPITests
{
    public class TransactionBuilderTests
    {
        [Fact]
        public void Validate_ReturnsFalseWhenTransactionDTOIsEmpty()
        {
            // Arrange
            var builder = new TransactionBuilder();
            var transactionData = new TransactionDTO()
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

            // Act
            var result = builder.Build(transactionData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transactionData.Id, result.Id.ToString());
            Assert.Equal(transactionData.ApplicationName, result.ApplicationName);
            Assert.Equal(transactionData.Email, result.Email);
            Assert.Equal(transactionData.Filename, result.Filename);
            Assert.Equal(transactionData.Url + "/", result.Url.AbsoluteUri);
            Assert.Equal(transactionData.Inception, result.Inception.ToString("M/d/yyyy"));
            Assert.Equal(transactionData.Amount, result.AmountCurrency + result.Amount.ToString());
            Assert.Equal(transactionData.Allocation, result.Allocation.ToString());
        }
    }
}
