using App.Betterboard.Api.Model;
using App.Betterboard.Tests.Unit.Builders;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Tests.Unit.Builders;

public interface IDefaultMemberBuilder : IMemberBuilder
{
    Member Build(UserBase fromUserBase);
}