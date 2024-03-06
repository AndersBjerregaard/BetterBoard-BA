namespace WebDriverXUnit.Domain.Exceptions;

public class IncompleteUserCredentialsException : Exception {
    public IncompleteUserCredentialsException(){}
    public IncompleteUserCredentialsException(string message) : base(message) {}
    public IncompleteUserCredentialsException(string message, Exception inner) : base(message, inner) {} 
}