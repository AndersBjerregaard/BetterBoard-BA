using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsWindow : IWindowDriver {
    void GoToBoard(string boardName, ref ITestOutputHelper testOutput);
    void AssertGotoBoard(string boardName);
    IBoardsAssertion AssertBoardHas(string boardName, ITestOutputHelper _testOutputHelper);
}