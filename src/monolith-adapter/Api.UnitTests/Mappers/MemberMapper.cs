using App.Betterboard.Api.Model;
using BetterBoard.Core.Model;

namespace App.Betterboard.Tests.Unit.Mappers;

public static class MemberMapper
{
    public static void MapUserBaseToMember(in UserBase userBase, ref Member member)
    {
        member.Created = userBase.Created;
        member.CreatedBy = userBase.CreatedBy;
        member._id = userBase._id;
        member.Name = userBase.Name;
        member.CountryCode = userBase.CountryCode;
        member.Phone = userBase.Phone;
        member.Email = userBase.Email;
        member.Company = userBase.Company;
        member.Title = userBase.Title;
        member.Picture = userBase.Picture;
        member.Lang = userBase.Lang;
        member.LastLoginTime = userBase.LastLoginTime;
        member.Deleted = userBase.Deleted;
        member.DeletedAt = userBase.DeletedAt;
        member.UseTwoFactor = userBase.UseTwoFactor;
    }
}