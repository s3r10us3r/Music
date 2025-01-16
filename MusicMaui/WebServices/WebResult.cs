namespace MusicMaui.WebServices
{
    public class WebResult<T>
    {
        public bool Succeeded { get; }
        public string ErrorMessage { get; }
        public T Data { get; }
        public bool Failed => !Succeeded;
        public int StatusCode { get; }

        private WebResult(bool succeeded, string errorMessage, T data, int statusCode)
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;
            Data = data;
            StatusCode = statusCode;
        }

        public static WebResult<T> Success(T data, int statusCode) => new WebResult<T>(true, null, data, statusCode);
        public static WebResult<T> Failure(string errorMessage, int statusCode) => new WebResult<T>(false, errorMessage, default, statusCode);
    }

    public class WebResult
    {
        public bool Succeeded { get; }
        public string ErrorMessage { get; }
        public bool Failed => !Succeeded;
        public int StatusCode { get; }
        private WebResult(bool succeeded, string errorMessage, int statusCode)
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
        public static WebResult Success(int statusCode) => new WebResult(true, null, statusCode);
        public static WebResult Failure(string errorMessage, int statusCode) => new WebResult(false, errorMessage, statusCode);
    }
}
