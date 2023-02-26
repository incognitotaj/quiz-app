namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null, object data = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(statusCode);
            Data = data;
        }

        private string GetDefaultMessage(int statusCode)
        {
            string message = string.Empty;

            switch (statusCode)
            {
                case 200:
                case 201:
                case 204:
                    IsError = false;
                    message = string.Empty;
                    break;
                case 400:
                    IsError = true;
                    message = "Bad request";
                    break;
                case 401:
                    IsError = true;
                    message = "Unauthorized";
                    break;
                case 404:
                    IsError = true;
                    message = "Resource not found";
                    break;
                case 500:
                    IsError = true;
                    message = "Server error";
                    break;
                default:
                    IsError = false;
                    message = string.Empty;
                    break;
            }

            return message;

        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public Object Data { get; set; }
    }
}
