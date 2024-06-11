using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using WebDriverXUnit.Domain;
using WebDriverXUnit.WindowDrivers.Interfaces;
using Xunit.Sdk;

namespace WebDriverXUnit.WindowDrivers;

public class LoginWindow(RemoteWebDriver driver, Uri baseUri) : ILoginWindow
{

    /// <summary></summary>
    /// <exception cref="Xunit.Sdk.NotNullException"></exception>
    /// <exception cref="Xunit.Sdk.NotEmptyException"></exception>
    /// <exception cref="OpenQA.Selenium.InvalidElementStateException"></exception>
    /// <exception cref="OpenQA.Selenium.ElementNotVisibleException"></exception>
    /// <exception cref="OpenQA.Selenium.StaleElementReferenceException"></exception>
    public void LoginOld(UserCredentials userCredentials)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var fields = wait.Until(d => {
            var elements = driver.FindElements(By.ClassName("form-control"));
            return elements.Any() ? elements : null;
        });
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

    public void Login(UserCredentials userCredentials) {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var fields = wait.Until(d => {
            var elements = driver.FindElements(By.XPath("//input[@placeholder='Email' or @placeholder='Password']"));
            return elements.Count == 2 ? elements : null;
        });
        fields?[0].SendKeys(userCredentials.Email);
        fields?[1].SendKeys(userCredentials.Password);
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var submit = wait.Until(d => {
            var e = driver.FindElement(By.XPath("//input[@type='submit']"));
            return e.Enabled ? e : null;
        });
        submit?.Click();
    }

    /// <summary></summary>
    /// <exception cref="Xunit.Sdk.NotNullException"></exception>
    /// <exception cref="Xunit.Sdk.ContainsException"></exception>
    public void AssertLogin(UserCredentials userCredentials)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException), typeof(ContainsException));
        wait.Until(driver => {
            var element = driver.FindElement(By.TagName("h2"));
            Assert.Contains(userCredentials.UserName.ToLower(), element.Text.ToLower());
            return true;
        });
    }

    public void Navigate()
    {
        driver.Navigate().GoToUrl(baseUri + "#/login");
    }

    /// <summary></summary>
    /// <exception cref="Xunit.Sdk.NotNullException"></exception>
    /// <exception cref="Xunit.Sdk.EqualException"></exception>
    public void AssertNavigation() {
        driver.Url.Should().Be(baseUri + "#/login");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        var header = wait.Until(driver => {
            var element = driver.FindElement(By.TagName("h4"));
            return element.Displayed ? element : null;
        });
        Assert.NotNull(header);
        Assert.Equal("Welcome to BetterBoard.", header.Text);
        Assert.Equal("BetterBoard - Board Management System", driver.Title);
    }

    public void FullLoginSession(UserCredentials userCredentials)
    {
        Navigate();
        AssertNavigation();
        Login(userCredentials);
        AssertLogin(userCredentials);
        driver.ExecuteScript("localStorage.setItem(arguments[0],arguments[1])", "purechat_expanded", "false"); // Disable help pop-up
    }
}