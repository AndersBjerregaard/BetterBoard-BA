using System;

namespace App.BetterBoard.Api.Helpers
{
    public static class ExceptionMessageHelper
    {
        public static string ConcatenateExceptionMessage(in Exception e)
        {
            string errorMessage = $"An exception of type {e.GetType().Name} occurred: {e.Message}.";
            string stackTrace = $"Stack Trace: {e.StackTrace}";

            if (e.InnerException != null)
            {
                errorMessage += $" Inner Exception: {e.InnerException.Message}.";
                stackTrace += $"\nInner Exception Stack Trace: {e.InnerException.StackTrace}";
            }

            return string.Join("\n", new string[] { errorMessage, stackTrace });
        }
    }
}