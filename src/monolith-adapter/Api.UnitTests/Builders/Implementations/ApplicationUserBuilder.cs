using App.Betterboard.Api.Model;
using App.Betterboard.Models;
using App.BetterBoard.Tests.Unit.Builders;
using App.Betterboard.Api.Extensions;
using App.Betterboard;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Tests.Unit.Builders.Implementation;

public class ApplicationUserBuilder : IDefaultBuilder<ApplicationUser>
{
    private readonly Member _baseMember;
    private readonly ApplicationUserManager _userManager;
    private readonly Guid _identifier;

    public ApplicationUserBuilder(Member fromMember, Guid identifier, in ApplicationUserManager userManager)
    {
        _baseMember = fromMember;
        _identifier = identifier;
        _userManager = userManager;
    }

    public ApplicationUser Build()
    {
        return new ApplicationUser
        {
            Id = _baseMember._id.ToString(),
            EmailAddress = _baseMember.Email,
            UserName = _baseMember.Email,
            PhoneNumber = _baseMember.Phone.TrimWhiteSpaces(),
            PasswordHash = _userManager.PasswordHasher.HashPassword(_identifier.ToString()),
            TwoFactorEnabled = false,
        };
    }
}