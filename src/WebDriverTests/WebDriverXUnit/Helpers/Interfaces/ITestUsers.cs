using WebDriverXUnit.Domain;

namespace WebDriverXUnit.Helpers.Interfaces;

public interface ITestUsers {
    UserCredentials GetTestUser();
}