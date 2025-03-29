using System.Net;
using System.Text.Json;


public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logFilePath;

    public ErrorLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "errorlog.txt");
        EnsureLogDirectoryExists();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await LogErrorToFile(context, ex);
            await HandleExceptionAsync(context,ex);
        }
    }

    private void EnsureLogDirectoryExists()
    {
        var logDirectory = Path.GetDirectoryName(_logFilePath);
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    private async Task LogErrorToFile(HttpContext context, Exception exception)
    {
        try
        {
            var logEntry = new
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                QueryString = context.Request.QueryString.ToString(),
                ErrorMessage = exception.Message,
            };

            string logText = JsonSerializer.Serialize(logEntry) + Environment.NewLine;

            await File.AppendAllTextAsync(_logFilePath, logText);
        }
        catch (Exception logEx)
        {
            Console.WriteLine($"Failed to write to log file: {logEx.Message}");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
            exception.Message, 
            Details = exception.StackTrace 
        };

        var errorJson = JsonSerializer.Serialize(errorResponse);

        return context.Response.WriteAsync(errorJson);
    }

}

