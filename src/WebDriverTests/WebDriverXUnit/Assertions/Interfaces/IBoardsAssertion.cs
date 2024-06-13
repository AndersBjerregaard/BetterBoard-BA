using WebDriverXUnit.Abstractions;

namespace WebDriverXUnit.Assertions.Interfaces;

public interface IBoardsAssertion
{
    IBoardsAssertion HasUnreadDocuments();
    IBoardsAssertion HasNoUnreadDocuments();
    IBoardsAssertion HasUnsignedDocuments();
    IBoardsAssertion HasNoUnsignedDocuments();
    IBoardsAssertion HasUpcomingMeeting();
    IBoardsAssertion HasNoUpcomingMeeting();
    IBoardsAssertion IsABoard();
    IBoardsAssertion IsADataroom();
}