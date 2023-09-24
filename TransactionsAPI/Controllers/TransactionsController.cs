using System.Globalization;
using System.Net;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using TransactionsAPI.Models;
using TransactionsAPI.Models.DTO;
using TransactionsAPI.Models.Validators.Interfaces;

namespace TransactionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionValidator _transactionValidator;
        private readonly ITransactionBuilder _transactionBuilder;
        private APIResponse response;

        public TransactionsController(ITransactionValidator transactionValidator, ITransactionBuilder transactionBuilder)
        {
            _transactionValidator = transactionValidator;
            _transactionBuilder = transactionBuilder;
            response = new APIResponse();
        }

        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UploadTransactionsCsv(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string>() { "File not selected or empty." };
                    response.Result = null;
                    return response;
                }

                var problematicTransactions = new List<string>();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
                {
                    var records = csv.GetRecords<TransactionDTO>();
                    foreach (var record in records)
                    {
                        if (_transactionValidator.Validate(record))
                            problematicTransactions.Add($"Unable to import transaction with ID = {record.Id}, \n");
                        var transaction = _transactionBuilder.Build(record);
                        // if transaction does not exist add
                        // if transaction exists use transactionUpdateValidator
                    }

                    return Ok("CSV file uploaded and processed successfully.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
