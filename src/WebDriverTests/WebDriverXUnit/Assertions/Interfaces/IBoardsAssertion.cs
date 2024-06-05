using WebDriverXUnit.Abstractions;

namespace WebDriverXUnit.Assertions.Interfaces;

public interface IBoardsAssertion
{
    IBoardsAssertion HasUnreadDocuments();
    IBoardsAssertion HasUnsignedDocuments();
    IBoardsAssertion HasUpcomingMeeting();
    IBoardsAssertion IsABoard();
    IBoardsAssertion IsADataroom();
}