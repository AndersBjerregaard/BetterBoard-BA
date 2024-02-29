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
    /// <exception cref="IncompleteUserCredentials"></exception>
    public UserCredentials GetTestUser()
    {
        // Expects a json object matching structure of UserCredentials.cs
        string? value = Environment.GetEnvironmentVariable("TEST_USERS") 
            ?? throw new IncompleteUserCredentials("Could not read environment variable. Environment variable TEST_USERS was unset.");

        UserCredentials? deserialized = JsonSerializer.Deserialize<UserCredentials>(value) 
            ?? throw new IncompleteUserCredentials("Error deserializing environment variable TEST_USERS");

        return deserialized;
    }
}