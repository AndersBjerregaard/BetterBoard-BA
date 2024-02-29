using OpenQA.Selenium.DevTools.V119.Tracing;
using WebDriverXUnit.Domain;
using WebDriverXUnit.Domain.Exceptions;
using WebDriverXUnit.Helpers.Interfaces;

namespace WebDriverXUnit.Helpers;

public class TestUsersFile : ITestUsers {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IncompleteUserCredentials"></exception>
    public UserCredentials GetTestUser() {
        Dictionary<string, string> configValues = new Dictionary<string, string>();

        KeyValuePair<string, string>[] keyValuePairs = ReadUserFile();
        int i = 0;
        while (i < keyValuePairs.Length) {
            configValues.Add(keyValuePairs[i].Key, keyValuePairs[i].Value);
            i++;
        }

        if (!configValues.ContainsKey("email") || !configValues.ContainsKey("password"))
            throw new IncompleteUserCredentials("Missing key-value pairs from user file");

        return new UserCredentials(configValues["email"], configValues["password"]);
    }

    private static KeyValuePair<string, string>[] ReadUserFile() {
        KeyValuePair<string, string>[] configValue = new KeyValuePair<string, string>[2];
        try
        {
            var file = "johntest.txt";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            using (StreamReader streamReader = new StreamReader(filePath)) {
                string? line;
                int i = 0;
                while ((line = streamReader.ReadLine()) is not null) {
                    string[]parts = line.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2) {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim().Trim('"');
                        configValue[i] = new KeyValuePair<string, string>(key, value);
                        i++;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Could not read file 'johntest.txt'");
            Console.WriteLine(ex.Message);
        }
        return configValue;
    }
}