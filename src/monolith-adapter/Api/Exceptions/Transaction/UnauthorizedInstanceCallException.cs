using System;

namespace App.BetterBoard.Api.Exceptions.Transaction
{
    /// <summary>
    /// Thrown when a class attempts to invoke a method on a restricted class.
    /// This is enforced by a direct reference to the allowed classes in the stack trace at runtime.
    /// </summary>
    public class UnauthorizedInstanceCallException : Exception
    {
        public UnauthorizedInstanceCallException() : base() { }
        public UnauthorizedInstanceCallException(string message) : base(message) { }
        public UnauthorizedInstanceCallException(string message, Exception innerException) : base(message, innerException) { }
    }
}