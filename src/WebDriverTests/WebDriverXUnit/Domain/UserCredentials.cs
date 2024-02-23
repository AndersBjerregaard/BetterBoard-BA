namespace WebDriverXUnit.Domain;

public class UserCredentials(string username, string password)
{
    private readonly string _username = username;
    private readonly string _password = password;

    public string Username { get => _username; }
    public string Password { get => _password; }
}