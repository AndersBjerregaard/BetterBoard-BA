using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.Helpers;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class SignatureProcessModalWindow(RemoteWebDriver driver) : ISignatureProcessModalWindow
{
    public void AssertModal()
    {
        var header = new WebElementFinder(driver).Find(By.XPath("//h5[@class='modal-title' and contains(text(),'Start new signature process')]"));
        Assert.NotNull(header);
        Assert.True(header.Displayed);
    }
}