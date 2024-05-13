using System.Diagnostics;
using System.Text.Json;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Domain.Exceptions;
using WebDriverXUnit.Helpers.Interfaces;

namespace WebDriverXUnit.Helpers;

public class TestUsersFile : ITestUsers {

    private static readonly string FILE_NAME = "TEST_USERS.json";

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IncompleteUserCredentialsException"></exception>
    public UserCredentials GetTestUser() {
        UserCredentials userCredentials = ReadUserFile() ?? 
            throw new IncompleteUserCredentialsException($"Null {nameof(UserCredentials)} return value from {nameof(ReadUserFile)}");

        return userCredentials;
    }

    private static UserCredentials? ReadUserFile() {
        UserCredentials? userCredentials = null;
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FILE_NAME);
        using (StreamReader streamReader = new StreamReader(filePath))
        {
            string? line;
            int i = 0;
            while ((line = streamReader.ReadLine()) is not null)
            {
                UserCredentials? deserialized = JsonSerializer.Deserialize<UserCredentials>(line) ??
                    throw new JsonException($"Error deserializing contents from file '{FILE_NAME}' into type '{nameof(UserCredentials)}'");
                userCredentials = deserialized;
                i++;
            }
        }
        return userCredentials;
    }
}