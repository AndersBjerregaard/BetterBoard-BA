using App.Betterboard.Api.Model;
using BetterBoard.Core.Model;

namespace App.Betterboard.Tests.Unit.Builders;

public interface IMemberBuilder : IBuilder<Member>
{
    IMemberBuilder FromUserBase(UserBase userBase);
    IMemberBuilder WithStatus(int status);
    IMemberBuilder BuildRole();
    IMemberBuilder WithRole(string role);
    IMemberBuilder BuildAccessLevel();
    IMemberBuilder WithAccessLevel(string accessLevel);
    IMemberBuilder BuildAssignment();
    IMemberBuilder WithAssignment(string assignment);
    IMemberBuilder WithSortOrder(int order);
    IMemberBuilder BuildAddedToBoard();
    IMemberBuilder WithAddedToBoard(DateTime? added);
}