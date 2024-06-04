using WebDriverXUnit.Abstractions;

namespace WebDriverXUnit.WindowDrivers.Interfaces;

public interface IBoardsAssertion {
    Result<bool> UnreadDocuments();
    Result<bool> UnsignedDocuments();
    Result<bool> UpcomingMeeting();
}