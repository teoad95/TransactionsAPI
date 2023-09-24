using System.Globalization;
using System.Net;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using TransactionsAPI.Models;
using TransactionsAPI.Models.DTO;
using TransactionsAPI.Models.Validators.Interfaces;
using TransactionsAPI.Repository.IRepository;

namespace TransactionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _db;
        private readonly IMapper _mapper;
        private readonly ITransactionValidator _transactionValidator;
        private readonly ITransactionBuilder _transactionBuilder;


        public TransactionsController(ITransactionRepository db, IMapper mapper, ITransactionValidator transactionValidator, ITransactionBuilder transactionBuilder)
        {
            _db = db;
            _mapper = mapper;
            _transactionValidator = transactionValidator;
            _transactionBuilder = transactionBuilder;
        }

        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UploadTransactionsCsv(IFormFile file)
        {
            var problematicTransactions = new List<string>();
            try
            {
                if (file == null || file.Length == 0)
                    return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { "File not selected or empty." }, null);
                

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
                {
                    var records = csv.GetRecords<TransactionDTO>();
                    foreach (var record in records)
                    {
                        if (!_transactionValidator.Validate(record))
                        {
                            problematicTransactions.Add($"Error: Unable to import transaction with ID = {record.Id} due to not correct data, \n");
                            continue;
                        }

                        var transaction = _transactionBuilder.Build(record);

                        try
                        {
                            await _db.UpdateOrCreateIfNotExistAsync(transaction);
                        }
                        catch (ArgumentException e)
                        {
                            problematicTransactions.Add($"Error: Unable to handle transaction with id = {record.Id} due to {e.Message}, \n");
                        }

                    }

                    return Ok(new APIResponse(HttpStatusCode.OK, true, problematicTransactions, "CSV file uploaded and processed."));
                }
            }
            catch (Exception e)
            {
                problematicTransactions.Add(e.Message);
                return new APIResponse(HttpStatusCode.BadRequest, false, problematicTransactions, null);
            }
        }

        [HttpGet("{id:Guid}", Name = "GetTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTransaction(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                    return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { "Use a valid Guid." }, null);
                

                var transaction = await _db.GetAsync(id);
                if (transaction == null)
                    return new APIResponse(HttpStatusCode.NotFound, false, new List<string>() { $"Unable to find a transaction with this Id = {id}." }, null);
                

                return Ok(new APIResponse(HttpStatusCode.OK, true, null, transaction));
            }
            catch (Exception e)
            {
                return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { e.ToString() }, null);
            }

        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteTransaction(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                    return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { "Use a valid Guid." }, null);
                

                var transaction = await _db.GetAsync(id);
                if (transaction == null)
                    return new APIResponse(HttpStatusCode.NotFound, false, new List<string>() { $"Unable to find a transaction with this Id = {id}." }, null);

                await _db.DeleteAsync(transaction);
                return Ok(new APIResponse(HttpStatusCode.NoContent, true, null, transaction));
            }
            catch (Exception e)
            {
                return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { e.ToString() }, null);
            }
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateTransaction(Guid id, [FromBody] TransactionUpdateDTO transactionUpdateDTO)
        {
            try
            {
                if (transactionUpdateDTO == null)
                    return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { "Please provide the updated transaction." }, null);

                var transaction = await _db.GetAsync(id);
                if (transaction == null)
                    return new APIResponse(HttpStatusCode.NotFound, false, new List<string>() { $"Unable to find a transaction with this Id = {id}." }, null);

                Transaction updatedTransaction = _mapper.Map<Transaction>(transactionUpdateDTO);
                updatedTransaction.Id = id;

                try
                {
                    await _db.UpdateOrCreateIfNotExistAsync(transaction);
                }
                catch (ArgumentException e)
                {
                    return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { $"Error: Unable to handle transaction with id = {id} due to {e.Message}, \n" }, null);
                }

                return Ok(new APIResponse(HttpStatusCode.OK, true, null, updatedTransaction));
            }
            catch (Exception e)
            {
                return new APIResponse(HttpStatusCode.BadRequest, false, new List<string>() { e.ToString() }, null);
            }
        }
    }
}
