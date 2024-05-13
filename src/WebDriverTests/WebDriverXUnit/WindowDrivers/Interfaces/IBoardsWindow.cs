namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsWindow {
    void GoToBoard(string boardName);
    void AssertGotoBoard(string boardName);
}