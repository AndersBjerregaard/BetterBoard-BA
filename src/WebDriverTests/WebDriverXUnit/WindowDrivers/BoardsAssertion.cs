using WebDriverXUnit.Abstractions;
using WebDriverXUnit.WindowDrivers.Interfaces;

namespace WebDriverXUnit.WindowDrivers;

public class BoardsAssertion : IBoardsAssertion
{
    public Result<bool> UnreadDocuments()
    {
        throw new NotImplementedException();
    }

    public Result<bool> UnsignedDocuments()
    {
        throw new NotImplementedException();
    }

    public Result<bool> UpcomingMeeting()
    {
        throw new NotImplementedException();
    }
}