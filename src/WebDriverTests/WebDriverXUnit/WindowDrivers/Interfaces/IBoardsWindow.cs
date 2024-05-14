using Xunit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsWindow {
    void GoToBoard(string boardName, ref ITestOutputHelper testOutput);
    void AssertGotoBoard(string boardName);
}