using WebDriverXUnit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsAssertion {
    void HasUnreadDocuments();
    void HasUnsignedDocuments();
    void HasUpcomingMeeting();
}