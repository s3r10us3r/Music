namespace Music.Helpers;

public class Result<T>
{
    public int StatusCode { get; }
    public string Message { get; }
    public T Data { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(int statusCode, string message, T data, bool isSuccess)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        IsSuccess = isSuccess;
    }

    public static Result<T> Success(int statusCode, T data)
        => new Result<T>(statusCode, "", data, true);

    public static Result<T> Failure(int statusCode, string message)
        => new Result<T>(statusCode, message, default, false);
}

public class Result
{
    public int StatusCode { get; }
    public string Message { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(int statusCode, string message, bool isSuccess)
    {
        StatusCode = statusCode;
        Message = message;
        IsSuccess = isSuccess;
    }

    public static Result Success(int statusCode)
        => new Result(statusCode, "", true);

    public static Result Failure(int statusCode, string message)
        => new Result(statusCode, message, false);
}