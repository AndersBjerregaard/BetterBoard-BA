using System.Text.Json;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Domain.Exceptions;
using WebDriverXUnit.Helpers.Interfaces;

namespace WebDriverXUnit.Helpers;

public class TestUsersEnvironment : ITestUsers
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IncompleteUserCredentialsException"></exception>
    public UserCredentials GetTestUser()
    {
        // Expects a json object matching structure of UserCredentials.cs
        string? value = Environment.GetEnvironmentVariable("TEST_USERS") 
            ?? throw new IncompleteUserCredentialsException("Could not read environment variable. Environment variable TEST_USERS was unset.");

        UserCredentials? deserialized = JsonSerializer.Deserialize<UserCredentials>(value) 
            ?? throw new IncompleteUserCredentialsException("Error deserializing environment variable TEST_USERS");

        return deserialized;
    }
}