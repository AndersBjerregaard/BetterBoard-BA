using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WebDriverXUnit.WindowDrivers.Interfaces;
using WebDriverXUnit.WindowDrivers;
using WebDriverXUnit.Domain;
using Xunit.Abstractions;

namespace WebDriverXUnit.Helpers
{
    public static class LoginSession
    {
        private static Dictionary<string, Cookie>? _cookies;

        /// <summary>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetUri"></param>
        /// <param name="userCredentials"></param>
        /// <returns>Whether the login session was cached or not.</returns>
        public static bool Login(RemoteWebDriver driver, Uri targetUri, UserCredentials userCredentials, ITestOutputHelper testOutput)
        {
            ILoginWindow loginWindow = new LoginWindow(driver, targetUri);

            loginWindow.Navigate();

            testOutput.WriteLine("Task {0} navigated to login", Task.CurrentId);

            if (_cookies is not null)
            {
                driver.Manage().Cookies.DeleteCookieNamed("ai_session");
                driver.Manage().Cookies.DeleteCookieNamed("ai_user");

                driver.Manage().Cookies.AddCookie(_cookies["ai_session"]);
                driver.Manage().Cookies.AddCookie(_cookies["ai_user"]);

                testOutput.WriteLine("Task {0} used cookies", Task.CurrentId);

                return true;
            }

            loginWindow.AssertNavigation();

            loginWindow.Login(userCredentials);
            loginWindow.AssertLogin(userCredentials);

            ICookieJar jar = driver.Manage().Cookies;
            Cookie session = jar.GetCookieNamed("ai_session");
            Cookie user = jar.GetCookieNamed("ai_user");
            _cookies = new Dictionary<string, Cookie>([
                new KeyValuePair<string, Cookie>("ai_session", new Cookie(session.Name, session.Value, session.Domain, session.Path, session.Expiry, session.Secure, session.IsHttpOnly, session.SameSite)),
                new KeyValuePair<string, Cookie>("ai_user", new Cookie(user.Name, user.Value, user.Domain, user.Path, user.Expiry, user.Secure, user.IsHttpOnly, user.SameSite))
                ]);

            testOutput.WriteLine("Task {0} logged in manually", Task.CurrentId);

            return false;
        }
    }
}
