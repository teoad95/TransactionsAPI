using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using TransactionsAPI.Controllers;
using TransactionsAPI.Models;
using TransactionsAPI.Models.DTO;
using TransactionsAPI.Models.Validators.Interfaces;
using TransactionsAPI.Repository.IRepository;
using TransactionsAPITests.DataHelper;
using Xunit;

namespace TransactionsAPITests
{
    public class TransactionsControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ITransactionValidator> transactionsValidator;
        private readonly Mock<ITransactionBuilder> transactionBuilder;

        public TransactionsControllerTests()
        {
            _mapper = new Mock<IMapper>();

            transactionsValidator = new Mock<ITransactionValidator>();
            transactionsValidator.Setup(x => x.Validate(It.IsAny<TransactionDTO?>())).Returns(true);

            transactionBuilder = new Mock<ITransactionBuilder>();
            transactionBuilder.Setup(x => x.Build(It.IsAny<TransactionDTO>())).Returns(new Transaction());
        }

        [Fact]
        public async Task UploadAction_ReturnsBadRequestWhenCSVIsNull()
        {
            // Arrange
            var repo = new Mock<ITransactionRepository>();
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);
            IFormFile csv = null;

            // Act
            var result = await controller.UploadTransactionsCsv(csv);

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
            var repo = new Mock<ITransactionRepository>();
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);
            var csv = new Mock<IFormFile>();
            csv.Setup(f => f.Length).Returns(0);

            // Act
            var result = await controller.UploadTransactionsCsv(csv.Object);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("File not selected or empty.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task GetAllTransactionsAction_ReturnsBadRequestWhenPageSizeLessThan1()
        {
            // Arrange
            var repo = new Mock<ITransactionRepository>();
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.GetTransactions(0, 1);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("Both the size and the number of the pages should be greater than 1.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task GetAllTransactionsAction_ReturnsBadRequestWhenPageNumberLessThan1()
        {
            // Arrange
            var repo = new Mock<ITransactionRepository>();
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.GetTransactions(1, 0);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("Both the size and the number of the pages should be greater than 1.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task GetAllTransactionsAction()
        {
            // Arrange
            var repo = new Mock<ITransactionRepository>();
            repo.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(TestDataHelper.GetFakeTransactionsList());
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.GetTransactions(2, 1);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Value.StatusCode);
            Assert.True(result.Value.IsSuccess);
            Assert.NotNull(result.Value.Result);
            Assert.Null(result.Value.ErrorMessages);
        }

        [Fact]
        public async Task GetTransactionAction_ReturnsBadRequestWhenIdIsNullOrEmpty()
        {
            // Arrange
            var repo = new Mock<ITransactionRepository>();
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.GetTransaction(Guid.Empty);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("Use a valid Guid.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task GetTransactionAction_ReturnsNotFoundWhenTransactionDoesNotExist()
        {
            // Arrange
            var transactionGuid = Guid.NewGuid();
            var repo = new Mock<ITransactionRepository>();
            repo.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Transaction?)null);

            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.GetTransaction(transactionGuid);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal($"Unable to find a transaction with this Id = {transactionGuid}.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task GetTransactionAction()
        {
            // Arrange
            var transactionGuid = Guid.NewGuid();
            var repo = new Mock<ITransactionRepository>();
            repo.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(TestDataHelper.GetFakeTransactionsList().First);

            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.GetTransaction(transactionGuid);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Value.StatusCode);
            Assert.True(result.Value.IsSuccess);
            Assert.NotNull(result.Value.Result);
            Assert.True(result.Value.ErrorMessages == null);
        }

        [Fact]
        public async Task DeleteTransactionAction_ReturnsBadRequestWhenIdIsNullOrEmpty()
        {
            // Arrange
            var repo = new Mock<ITransactionRepository>();
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.DeleteTransaction(Guid.Empty);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("Use a valid Guid.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task DeleteTransactionAction_ReturnsNotFoundWhenTransactionDoesNotExist()
        {
            // Arrange
            var transactionGuid = Guid.NewGuid();
            var repo = new Mock<ITransactionRepository>();
            repo.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Transaction?)null);

            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.DeleteTransaction(transactionGuid);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal($"Unable to find a transaction with this Id = {transactionGuid}.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task DeleteTransactionAction()
        {
            // Arrange
            var transactionGuid = Guid.NewGuid();
            var repo = new Mock<ITransactionRepository>();
            repo.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(TestDataHelper.GetFakeTransactionsList().First);

            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.DeleteTransaction(transactionGuid);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Value.StatusCode);
            Assert.True(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages == null);
        }

        [Fact]
        public async Task UpdateTransactionAction_ReturnsBadRequestWhenTransactionUpdateDTOIsNull()
        {
            // Arrange
            var repo = new Mock<ITransactionRepository>();
            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.UpdateTransaction(Guid.Empty, null);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal("Please provide the updated transaction.", result.Value.ErrorMessages.First());
        }

        [Fact]
        public async Task UpdateTransactionAction_ReturnsNotFoundWhenTransactionDoesNotExist()
        {
            // Arrange
            var transactionGuid = Guid.NewGuid();
            var repo = new Mock<ITransactionRepository>();
            repo.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Transaction?)null);

            var controller = new TransactionsController(repo.Object, _mapper.Object, transactionsValidator.Object, transactionBuilder.Object);

            // Act
            var result = await controller.UpdateTransaction(transactionGuid, new TransactionUpdateDTO());

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Value.StatusCode);
            Assert.False(result.Value.IsSuccess);
            Assert.Null(result.Value.Result);
            Assert.True(result.Value.ErrorMessages != null && result.Value.ErrorMessages.Count == 1);
            Assert.Equal($"Unable to find a transaction with this Id = {transactionGuid}.", result.Value.ErrorMessages.First());
        }

    }
}
