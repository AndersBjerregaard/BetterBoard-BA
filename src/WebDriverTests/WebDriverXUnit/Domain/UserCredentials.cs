namespace WebDriverXUnit.Domain;

public class UserCredentials(string email, string password)
{
    private readonly string _email = email;
    private readonly string _password = password;

    public string Email { get => _email; }
    public string Password { get => _password; }
    public string UserName { get { 
        return  "Test User - " + _email.Split('@')[0];
        }
    }

    public override string ToString()
    {
        return $"Email: {_email} Password: {_password}";
    }
}