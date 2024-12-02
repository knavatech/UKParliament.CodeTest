using System.IO;
using System.Text.Json;
using UKParliament.CodeTest.API.Helper;

namespace UKParliament.CodeTest.API.Middleware;
//TODO: this result wraper is not fully completed 
//if the contentType is not JSON, we have to wrap the result in the controller for now
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

            context.Response.Body = originalBodyStream;

            if (IsAlreadyWrapped(bodyText))
            {
                await newBodyStream.CopyToAsync(originalBodyStream);
                return;
            }

            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                await WrapResponse(context , context.Response.StatusCode
                                , string.IsNullOrWhiteSpace(bodyText) ? null : JsonSerializer.Deserialize<object>(bodyText));
            }
            else
            {
                await WrapResponse(context);
            }
        }
        catch (Exception ex)
        {
            //TODO: proper error handling
            await WrapResponse(context, StatusCodes.Status500InternalServerError);
            await ResetAndCopyModifiedStreamToOriginal(newBodyStream, originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
            newBodyStream.Position = 0;
            await newBodyStream.FlushAsync();
            newBodyStream.Close();
        }
    }

    private async Task ResetAndCopyModifiedStreamToOriginal(MemoryStream memoryStream, Stream originalBody)
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        // Reset the position and copy the modified response back to the original stream
        await memoryStream.CopyToAsync(originalBody);
    }

    private async Task WrapResponse(HttpContext context, int? statusCode = null, object? data = null)
    {
        int currentStatusCode = statusCode ?? context.Response.StatusCode;
        var wrappedResponse = new Result<object>(data, GetDefaultMessageForStatusCode(currentStatusCode), currentStatusCode);

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedResponse));
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
