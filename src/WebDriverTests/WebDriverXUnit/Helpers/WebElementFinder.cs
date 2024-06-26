using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace WebDriverXUnit.Helpers;

public class WebElementFinderOptions {
    public TimeSpan Wait { get; set; } = TimeSpan.FromSeconds(30);
    public Type[] IgnoreExceptionTypes { get; set; } = [typeof(NoSuchElementException), typeof(StaleElementReferenceException)];
}

public class WebElementFinder {
    private readonly RemoteWebDriver _driver;

    public TimeSpan Wait { get; set; } = TimeSpan.FromSeconds(30);
    public Type[] IgnoreExceptionTypes { get; set; } = [typeof(NoSuchElementException), typeof(StaleElementReferenceException)];

    public WebElementFinder(RemoteWebDriver driver) {
        _driver = driver;
    }
    public WebElementFinder(RemoteWebDriver driver, Action<WebElementFinderOptions> configureOptions) {
        _driver = driver;

        var options = new WebElementFinderOptions();
        configureOptions(options);
        Wait = options.Wait;
        IgnoreExceptionTypes = options.IgnoreExceptionTypes;
    }

    public IWebElement? Find(By by) {
        var wait = new WebDriverWait(_driver, Wait);
        wait.IgnoreExceptionTypes(IgnoreExceptionTypes);
        var element = wait.Until(d => {
            var element = _driver.FindElement(by);
            return element.Displayed ? element : null;
        });
        return element;
    }

    public ReadOnlyCollection<IWebElement>? FindMultiple(By by) {
        var wait = new WebDriverWait(_driver, Wait);
        wait.IgnoreExceptionTypes(IgnoreExceptionTypes);
        var elements = wait.Until(d => {
            var elements = _driver.FindElements(by);
            return elements.Any() ? elements : null;
        });
        return elements;
    }
}