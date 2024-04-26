using App.Betterboard.Api.Model;
using BetterBoard.Core.Model;
using MongoDB.Bson;

namespace App.Betterboard.Tests.Unit.Builders;

public interface IUserBaseBuilder : IBuilder<UserBase>
{
    IUserBaseBuilder BuildCreated();
    IUserBaseBuilder WithCreated(DateTime created);
    IUserBaseBuilder BuildCreatedBy();
    IUserBaseBuilder WithCreatedBy(ObjectId userId);
    IUserBaseBuilder BuildId();
    IUserBaseBuilder WithId(ObjectId id);
    IUserBaseBuilder BuildName();
    IUserBaseBuilder WithName(string name);
    IUserBaseBuilder BuildCountryCode();
    IUserBaseBuilder WithCountryCode(string countryCode);
    IUserBaseBuilder BuildPhone();
    IUserBaseBuilder WithPhone(string phone);
    IUserBaseBuilder BuildEmail();
    IUserBaseBuilder WithEmail(string email);
    IUserBaseBuilder BuildCompany();
    IUserBaseBuilder WithCompany(string company);
    IUserBaseBuilder BuildTitle();
    IUserBaseBuilder WithTitle(string title);
    IUserBaseBuilder BuildLang();
    IUserBaseBuilder WithLang(string lang);
    IUserBaseBuilder BuildLastLoginTime();
    IUserBaseBuilder WithLastLoginTime(DateTime lastLogin);
    IUserBaseBuilder BuildUseTwoFactor();
    IUserBaseBuilder WithUseTwoFactor(bool twoFactor);
}