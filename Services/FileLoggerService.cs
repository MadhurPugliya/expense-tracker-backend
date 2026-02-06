using System.Text.Json;

namespace ExpenseTracker.Services
{
    public interface IFileLoggerService
    {
        Task LogAsync(string level, string message, object? data = null);
        Task LogErrorAsync(string message, Exception exception, object? data = null);
        Task LogInformationAsync(string message, object? data = null);
        Task LogWarningAsync(string message, object? data = null);
    }

    public class FileLoggerService : IFileLoggerService
    {
        private readonly string _logDirectory;
        private readonly string _logFileName;

        public FileLoggerService()
        {
            _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            _logFileName = $"expense-tracker-{DateTime.Now:yyyy-MM-dd}.log";
            
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public async Task LogAsync(string level, string message, object? data = null)
        {
            var logEntry = new
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Level = level,
                Message = message,
                Data = data
            };

            var logText = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
            var logFilePath = Path.Combine(_logDirectory, _logFileName);

            await File.AppendAllTextAsync(logFilePath, logText + Environment.NewLine);
        }

        public async Task LogErrorAsync(string message, Exception exception, object? data = null)
        {
            var errorData = new
            {
                ExceptionMessage = exception.Message,
                StackTrace = exception.StackTrace,
                AdditionalData = data
            };

            await LogAsync("ERROR", message, errorData);
        }

        public async Task LogInformationAsync(string message, object? data = null)
        {
            await LogAsync("INFO", message, data);
        }

        public async Task LogWarningAsync(string message, object? data = null)
        {
            await LogAsync("WARNING", message, data);
        }
    }
}