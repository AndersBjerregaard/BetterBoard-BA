using OpenQA.Selenium.DevTools.V119.Tracing;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Domain.Exceptions;

namespace WebDriverXUnit.Helpers;

public static class TestUsers {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IncompleteUserCredentials"></exception>
    public static UserCredentials GetTestUser() {
        var configValues = ReadUserFile();
        if (!configValues.ContainsKey("email") || !configValues.ContainsKey("password"))
            throw new IncompleteUserCredentials("Missing key-value pairs from user file");
        return new UserCredentials(configValues["email"], configValues["password"]);
    }

    private static Dictionary<string, string> ReadUserFile() {
        Dictionary<string, string> configValues = new Dictionary<string, string>();
        try
        {
            using (StreamReader streamReader = new StreamReader(@"../johntest.txt")) {
                string? line;
                while ((line = streamReader.ReadLine()) is not null) {
                    string[]parts = line.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2) {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim().Trim('"');
                        configValues[key] = value;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Could not read file 'johntest.txt'");
            Console.WriteLine(ex.Message);
        }
        return configValues;
    }
}