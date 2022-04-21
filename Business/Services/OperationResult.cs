using System.Collections.Generic;

namespace Customers.Business
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }

        public OperationResult(T result)
        {
            Result = result;
            Success = true;
        }

        public OperationResult(T result, string error, string errorMessage)
        {
            Result = result;
            Error = error;
            ErrorMessage = errorMessage;
            Success = false;
        }
    }
}
