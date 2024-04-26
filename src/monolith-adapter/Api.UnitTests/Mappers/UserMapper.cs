using App.Betterboard.Api.Model;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Tests.Unit.Mappers;

public static class UserMapper
{
    public static void MapUserBaseToUser(in UserBase fromUserBase, ref User toUser)
    {
        toUser.Created = fromUserBase.Created;
        toUser.CreatedBy = fromUserBase.CreatedBy;
        toUser._id = fromUserBase._id;
        toUser.Name = fromUserBase.Name;
        toUser.CountryCode = fromUserBase.CountryCode;
        toUser.Phone = fromUserBase.Phone;
        toUser.Email = fromUserBase.Email;
        toUser.Company = fromUserBase.Company;
        toUser.Title = fromUserBase.Title;
        toUser.Picture = fromUserBase.Picture;
        toUser.Lang = fromUserBase.Lang;
        toUser.LastLoginTime = fromUserBase.LastLoginTime;
        toUser.Deleted = fromUserBase.Deleted;
        toUser.DeletedAt = fromUserBase.DeletedAt;
        toUser.UseTwoFactor = fromUserBase.UseTwoFactor;
    }
}