using System.Net;

namespace TransactionsAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages = new List<string>();
        public object Result { get; set; }

        public APIResponse(HttpStatusCode statusCode, bool isSuccess, List<string> errorMessages , object result)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            ErrorMessages = errorMessages;
            Result = result;
        }
    }
}
