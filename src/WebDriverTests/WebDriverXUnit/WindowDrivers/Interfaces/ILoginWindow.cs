using WebDriverXUnit.Domain;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface ILoginWindow : IWindowDriver {
    void Login(UserCredentials userCredentials);
    void AssertLogin(UserCredentials userCredentials);
    void FullLoginSession(UserCredentials userCredentials);
}