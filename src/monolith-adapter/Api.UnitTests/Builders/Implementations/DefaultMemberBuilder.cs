using App.Betterboard.Api.Model;
using App.Betterboard.Tests.Unit.Builders.Implementations;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Tests.Unit.Builders.Implementation;

public class DefaultMemberBuilder : MemberBuilder, IDefaultMemberBuilder
{
    public Member Build(UserBase fromUserBase)
    {
        this.FromUserBase(fromUserBase);
        this.BuildRole();
        this.BuildAccessLevel();
        this.BuildAssignment();
        this.BuildAddedToBoard();
        return this.GetResult();
    }
}