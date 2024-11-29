using System.Text.Json;
using UKParliament.CodeTest.API.Helper;

namespace UKParliament.CodeTest.API.Middleware;
//TODO: this result wraper is not fully completed 
public class ResponseWrapperMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseWrapperMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        using var newBodyStream = new MemoryStream();

        try
        {
            context.Response.Body = newBodyStream;

            await _next(context);

            newBodyStream.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(newBodyStream).ReadToEndAsync();

            newBodyStream.Seek(0, SeekOrigin.Begin);
            context.Response.Body = originalBodyStream;

            if (IsAlreadyWrapped(bodyText) || !IsJsonResponse(context))
            {
                await newBodyStream.CopyToAsync(originalBodyStream);
                return;
            }

            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                var wrappedNotFoundResponse = new Result<object>(
                    string.IsNullOrEmpty(bodyText) ? null : JsonSerializer.Deserialize<object>(bodyText),
                    GetDefaultMessageForStatusCode(context.Response.StatusCode)
                    , context.Response.StatusCode);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedNotFoundResponse));
                return;
            }

            var wrappedResponse = new Result<object>(null, GetDefaultMessageForStatusCode(context.Response.StatusCode),
                context.Response.StatusCode);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedResponse));
        }
        catch (Exception ex)
        {
            //TODO: proper error handling

            var wrappedNotFoundResponse = new Result<object>(null, GetDefaultMessageForStatusCode(StatusCodes.Status500InternalServerError)
                , StatusCodes.Status500InternalServerError);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedNotFoundResponse));
            return;

        }
        finally
        {
            context.Response.Body = originalBodyStream;
            newBodyStream.Position = 0;
            await newBodyStream.FlushAsync();
            newBodyStream.Close();
        }
    }

    private static bool IsAlreadyWrapped(string bodyText)
    {
        if (string.IsNullOrEmpty(bodyText)) return false;

        try
        {
            using var jsonDoc = JsonDocument.Parse(bodyText);
            return jsonDoc.RootElement.TryGetProperty("Data", out _) &&
                   jsonDoc.RootElement.TryGetProperty("Message", out _) &&
                   jsonDoc.RootElement.TryGetProperty("StatusCode", out _);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsJsonResponse(HttpContext context)
    {
        return context.Response.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        switch (statusCode)
        {
            case StatusCodes.Status200OK:
            case StatusCodes.Status201Created:
            case StatusCodes.Status204NoContent:
            case StatusCodes.Status202Accepted:
                return "Request successful";
            case StatusCodes.Status400BadRequest:
                return "Bad request";
            case StatusCodes.Status404NotFound:
                return "Resource not found";
            case StatusCodes.Status500InternalServerError:
                return "Internal server error";
            default:
                return "Unhandled response status";
        }
    }
}
