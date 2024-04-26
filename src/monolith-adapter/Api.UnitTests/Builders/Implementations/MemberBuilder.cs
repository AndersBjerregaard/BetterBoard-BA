using App.Betterboard.Api.Model;
using App.Betterboard.Api.Services;
using App.Betterboard.Tests.Unit.Mappers;
using BetterBoard.Core.Model;

namespace App.Betterboard.Tests.Unit.Builders.Implementations;

public class MemberBuilder : IMemberBuilder
{
    private protected Member _member = new Member();
    
    public Member GetResult()
    {
        return _member;
    }

    public IMemberBuilder FromUserBase(UserBase userBase)
    {
        MemberMapper.MapUserBaseToMember(userBase, ref _member);
        return this;
    }

    public IMemberBuilder WithStatus(int status)
    {
        _member.Status = status;
        return this;
    }

    public virtual IMemberBuilder BuildRole()
    {
        _member.Role = CONSTANTS.ROLE_BOARDMEMBER;
        return this;
    }

    public IMemberBuilder WithRole(string role)
    {
        _member.Role = role;
        return this;
    }

    public virtual IMemberBuilder BuildAccessLevel()
    {
        _member.AccessLevel = CONSTANTS.ACL_USERLVL2;
        return this;
    }

    public IMemberBuilder WithAccessLevel(string accessLevel)
    {
        _member.AccessLevel = accessLevel;
        return this;
    }

    public virtual IMemberBuilder BuildAssignment()
    {
        _member.Assignment = string.Empty;
        return this;
    }

    public IMemberBuilder WithAssignment(string assignment)
    {
        _member.Assignment = assignment;
        return this;
    }

    public IMemberBuilder WithSortOrder(int order)
    {
        _member.SortOrder = order;
        return this;
    }

    public virtual IMemberBuilder BuildAddedToBoard()
    {
        _member.AddedToBoard = DateTime.Now;
        return this;
    }

    public IMemberBuilder WithAddedToBoard(DateTime? added)
    {
        _member.AddedToBoard = added;
        return this;
    }
}