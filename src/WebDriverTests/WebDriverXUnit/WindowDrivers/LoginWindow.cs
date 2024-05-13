using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Domain;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class LoginWindow(RemoteWebDriver driver, Uri baseUri) : ILoginWindow
{

    /// <summary></summary>
    /// <exception cref="Xunit.Sdk.NotNullException"></exception>
    /// <exception cref="Xunit.Sdk.NotEmptyException"></exception>
    /// <exception cref="OpenQA.Selenium.InvalidElementStateException"></exception>
    /// <exception cref="OpenQA.Selenium.ElementNotVisibleException"></exception>
    /// <exception cref="OpenQA.Selenium.StaleElementReferenceException"></exception>
    public void Login(UserCredentials userCredentials)
    {
        var fields = driver.FindElements(By.ClassName("form-control"));
        Assert.NotNull(fields);
        Assert.NotEmpty(fields);
        var submit = driver.FindElement(By.ClassName("btn-primary"));
        Assert.NotNull(submit);

        var usernameField = fields[0];
        usernameField.SendKeys(userCredentials.Email);
        var passwordField = fields[1];
        passwordField.SendKeys(userCredentials.Password);

        submit.Click();
    }

    /// <summary></summary>
    /// <exception cref="Xunit.Sdk.NotNullException"></exception>
    /// <exception cref="Xunit.Sdk.ContainsException"></exception>
    public void AssertLogin(UserCredentials userCredentials)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var boardHeader = wait.Until(driver => {
            var element = driver.FindElement(By.TagName("h2"));
            return element.Displayed ? element : null;
        });
        Assert.NotNull(boardHeader);
        Assert.Contains(userCredentials.UserName, boardHeader.Text);
    }

    public void Navigate()
    {
        driver.Navigate().GoToUrl(baseUri + "#/login");
    }

    /// <summary></summary>
    /// <exception cref="Xunit.Sdk.NotNullException"></exception>
    /// <exception cref="Xunit.Sdk.EqualException"></exception>
    public void AssertNavigation() {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var header = wait.Until(driver => {
            var element = driver.FindElement(By.TagName("h4"));
            return element.Displayed ? element : null;
        });
        Assert.NotNull(header);
        Assert.Equal("Welcome to BetterBoard.", header.Text);
        Assert.Equal("BetterBoard - Board Management System", driver.Title);
    }
}