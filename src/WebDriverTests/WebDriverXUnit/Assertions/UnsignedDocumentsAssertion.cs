using OpenQA.Selenium;
using WebDriverXUnit.Assertions.Interfaces;
using WebDriverXUnit.Helpers;
using Xunit.Abstractions;

namespace WebDriverXUnit.Assertions;

public class UnsginedDocumentsAssertion(IWebElement table, ITestOutputHelper testOutput) : IUnsignedDocumentsAssertion
{
    public IUnsignedDocumentsAssertion HasDocumentFromOrigin(string origin)
    {
        var columns = table.FindElements(By.XPath(".//td[@class='col-md-2 hidden-xs']"));
        Assert.NotNull(columns);
        Assert.True(columns.Any());
        Assert.Contains(columns, x => x.Text.Contains(origin));
        return this;
    }

    public IUnsignedDocumentsAssertion HasDocumentFromOrigin(params string[] origins) {
        var columns = table.FindElements(By.XPath(".//td[@class='col-md-2 hidden-xs']"));
        Assert.NotNull(columns);
        Assert.True(columns.Any());
        var innerText = columns.Select(x => x.Text);
        bool multipleContains = origins.All(o => innerText.Any(s => s.Contains(o)));
        Assert.True(multipleContains);
        return this;
    }

    public IUnsignedDocumentsAssertion HasNoDocumentFromorigin(string origin)
    {
        var columns = table.FindElements(By.XPath(".//td[@class='col-md-2 hidden-xs']"));
        Assert.NotNull(columns);
        Assert.True(columns.Any());
        Assert.DoesNotContain(columns, x => x.Text.Contains(origin));
        return this;
    }

}