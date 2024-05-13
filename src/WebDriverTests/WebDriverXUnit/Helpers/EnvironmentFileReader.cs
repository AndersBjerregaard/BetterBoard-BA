using System.Text.Json;
using WebDriverXUnit.Domain;

namespace WebDriverXUnit.Helpers;

public static class EnvironmentFileReader {
    private static readonly string FILE_NAME = "ENVIRONMENT.json";

    private static EnvironmentSettings? _settings;
    public static EnvironmentSettings? Settings { get {
        _settings ??= ReadEnvironmentFile();
        return _settings;
    } }

    public static EnvironmentSettings? ReadEnvironmentFile() {
        EnvironmentSettings? settings = null;
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FILE_NAME);
        using (StreamReader reader = new StreamReader(filePath)) {
            string? line;
            int i = 0;
            while ((line = reader.ReadLine()) is not null) {
                EnvironmentSettings? deserialized = JsonSerializer.Deserialize<EnvironmentSettings>(line) ??
                    throw new JsonException($"Error deserializing contents from file '{filePath}' into type '{nameof(EnvironmentSettings)}'");
                settings = deserialized;
                i++;
            }
        }
        return settings;
    }
}