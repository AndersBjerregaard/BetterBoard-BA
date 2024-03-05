using Xunit.Abstractions;

namespace WebDriverXUnit.Helpers;

public static class ExceptionLogger {
    public static void LogException(Exception e, ref ITestOutputHelper logger) {
        string errorMessage = $"An exception of type {e.GetType().Name} occurred: {e.Message}.";
        string stackTrace = $"Stack Trace: {e.StackTrace}";

        // If there's an inner exception, include its message and stack trace
        if (e.InnerException != null)
        {
            errorMessage += $" Inner Exception: {e.InnerException.Message}.";
            stackTrace += $"\nInner Exception Stack Trace: {e.InnerException.StackTrace}";
        }

        // Log the error message and stack trace
        logger.WriteLine(errorMessage);
        logger.WriteLine(stackTrace);
    }
}