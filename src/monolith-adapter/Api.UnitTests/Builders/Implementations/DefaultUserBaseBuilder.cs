using App.Betterboard.Api.Model;
using App.Betterboard.Tests.Unit.Builders.Implementations;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Tests.Unit.Builders.Implementation;

public class DefaultUserBaseBuilder : UserBaseBuilder, IDefaultBuilder<UserBase>
{
    public UserBase Build()
    {
        this.BuildId();
        this.BuildCreated();
        this.BuildCreatedBy();
        this.BuildName();
        this.BuildCountryCode();
        this.BuildPhone();
        this.BuildEmail();
        this.BuildCompany();
        this.BuildTitle();
        this.BuildLang();
        this.BuildLastLoginTime();
        this.WithUseTwoFactor(false);
        return this.GetResult();
    }
}