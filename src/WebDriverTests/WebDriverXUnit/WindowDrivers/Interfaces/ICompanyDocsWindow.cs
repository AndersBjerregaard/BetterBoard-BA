namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface ICompanyDocsWindow {
    void AssertPage();
    void OpenFolder(string folderName);
    void OpenSignProcessModal(string documentName);
}