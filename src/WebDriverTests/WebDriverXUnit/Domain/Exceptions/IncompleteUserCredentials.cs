namespace WebDriverXUnit.Domain.Exceptions;

public class IncompleteUserCredentials : Exception {
    public IncompleteUserCredentials(){}
    public IncompleteUserCredentials(string message) : base(message) {}
    public IncompleteUserCredentials(string message, Exception inner) : base(message, inner) {} 
}