
namespace UKParliament.CodeTest.API.Helper;

public class Result<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public bool Success => StatusCode >= 200 && StatusCode < 300;

    public Result()
    {
        
    }

    public Result(T data, string message, int statusCode)
    {
        Data = data;
        Message = message;
        StatusCode = statusCode;
    }

    public static Result<T> SuccessResult(T data, string message = "Success")
    {
        return new(data, message, StatusCodes.Status200OK);
    }

    public static Result<T> SuccessResult(T data, string message = "Success", int statusCode = StatusCodes.Status200OK)
    {
        return new(data, message, statusCode);
    }

    public static Result<T> ErrorResult(string message = "An error occurred. Please contact your system administrator for assistance.", int statusCode = StatusCodes.Status500InternalServerError)
    {
       return new(default, message, statusCode);
    }

    public static Result<T> NoResult(string message = "Result not found")
    {
        return new(default, message, StatusCodes.Status404NotFound);
    }
}
