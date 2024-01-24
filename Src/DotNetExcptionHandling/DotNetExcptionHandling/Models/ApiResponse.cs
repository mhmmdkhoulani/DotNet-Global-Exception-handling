namespace DotNetExcptionHandling.Models
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public ApiResponse(string message)
        {
            IsSuccess = true;
            Message = message;
        }

        public ApiResponse()
        {
            IsSuccess = true;
        }
    }

    public class ApiResponse<T>
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }
        public T Value { get; set; }

        public ApiResponse()
        {
            IsSuccess = true;
        }

        public ApiResponse(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        public ApiResponse(T value, string message)
        {
            IsSuccess = true;
            Value = value;
            Message = message;
        }

    }
}
