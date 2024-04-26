using System;

namespace App.BetterBoard.Api.Exceptions
{
    public class InvalidMemberException : Exception
    {
        public InvalidMemberException() { }
        public InvalidMemberException(string message) : base(message) { }
        public InvalidMemberException(string message, in Exception innerException) : base(message, innerException) { }
    }
}