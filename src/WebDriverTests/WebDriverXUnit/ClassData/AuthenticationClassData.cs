using System.Collections;
using OpenQA.Selenium;

namespace WebDriverXUnit.ClassData;

public class AuthenticationClassData : IEnumerable<object[]>
{
    readonly DriverOptions[] driverOptions = Helpers.AvailableDriverOptions.Get();

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (DriverOptions driverOption in driverOptions)
        {
            yield return [driverOption];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}