using App.Betterboard.Api.Model;
using App.BetterBoard.Tests.Unit.Mappers;
using BetterBoard.Core.Model;
using MongoDB.Bson;

namespace App.BetterBoard.Tests.Unit.Builders.Implementation;

public class DefaultUserBuilder : IDefaultBuilder<User>
{
    private readonly UserBase fromUserBase;

    public DefaultUserBuilder(UserBase fromUserBase)
    {
        this.fromUserBase = fromUserBase;
    }

    public User Build()
    {
        var user = new User();
        UserMapper.MapUserBaseToUser(fromUserBase, ref user);
        user.identity = ObjectId.GenerateNewId();
        user.EulaAccepted = DateTime.Now;
        user.UserHasToChangePassword = false;
        user.Permission = true;
        user.Categories = new List<Category>();
        user.ReadList = new List<ReadDocument>();
        user.GDPRAccepted = DateTime.Now;
        return user;
    }
}
