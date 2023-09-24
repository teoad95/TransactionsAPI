using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TransactionsAPI.Controllers;
using TransactionsAPI.Models;
using TransactionsAPI.Models.DTO;
using TransactionsAPI.Models.Validators.Interfaces;
using Xunit;

namespace TransactionsAPITests
{
    public class TransactionsControllerTests
    {
        private readonly TransactionsController _controller;

        public TransactionsControllerTests()
        {
            var transactionsValidator = new Mock<ITransactionValidator>();
            transactionsValidator.Setup(x=> x.Validate(It.IsAny<TransactionDTO?>())).Returns(true);

            var transactionBuilder = new Mock<ITransactionBuilder>();
            transactionBuilder.Setup(x => x.Build(It.IsAny<TransactionDTO>())).Returns(new Transaction());

            _controller = new TransactionsController(transactionsValidator.Object, transactionBuilder.Object);
        }

        [Fact]
        public async Task UploadAction_ReturnsBadRequestWhenCSVIsNull()
        {
            // Arrange
            IFormFile csv = null;

            // Act
            var result = await _controller.UploadTransactionsCsv(csv);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("File not selected or empty.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task UploadAction_ReturnsBadRequestWhenCSVIsEmpty()
        {
            // Arrange
            var csv = new Mock<IFormFile>();
            csv.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _controller.UploadTransactionsCsv(csv.Object);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("File not selected or empty.", result.Value.ErrorMessages.First());
        }
    }
}
