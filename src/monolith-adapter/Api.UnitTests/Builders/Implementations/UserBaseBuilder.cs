using App.Betterboard.Api.Model;
using BetterBoard.Core.Model;
using MongoDB.Bson;

namespace App.Betterboard.Tests.Unit.Builders.Implementations;

public class UserBaseBuilder : IUserBaseBuilder
{
    private UserBase _userBase = new UserBase();
    private readonly Random _random = new Random();

    public UserBase GetResult()
    {
        return _userBase;
    }

    public IUserBaseBuilder BuildCreated()
    {
        _userBase.Created = DateTime.Now;
        return this;
    }

    public IUserBaseBuilder WithCreated(DateTime created)
    {
        _userBase.Created = created;
        return this;
    }

    public IUserBaseBuilder BuildCreatedBy()
    {
        _userBase.CreatedBy = ObjectId.GenerateNewId();
        return this;
    }

    public IUserBaseBuilder WithCreatedBy(ObjectId userId)
    {
        _userBase.CreatedBy = userId;
        return this;
    }

    public IUserBaseBuilder BuildId()
    {
        _userBase._id = ObjectId.GenerateNewId();
        return this;
    }

    public IUserBaseBuilder WithId(ObjectId id)
    {
        _userBase._id = id;
        return this;
    }

    public IUserBaseBuilder BuildName()
    {
        _userBase.Name = string.Empty;
        return this;
    }

    public IUserBaseBuilder WithName(string name)
    {
        _userBase.Name = name;
        return this;
    }

    public IUserBaseBuilder BuildCountryCode()
    {
        _userBase.CountryCode = "+45";
        return this;
    }

    public IUserBaseBuilder WithCountryCode(string countryCode)
    {
        _userBase.CountryCode = countryCode;
        return this;
    }

    public IUserBaseBuilder BuildPhone()
    {
        _userBase.Phone = _random.Next(10000000, 99999999).ToString();
        return this;
    }

    public IUserBaseBuilder WithPhone(string phone)
    {
        _userBase.Phone = phone;
        return this;
    }

    public IUserBaseBuilder BuildEmail()
    {
        _userBase.Email = $"user{_random.Next(1, 1000)}@mail.dk";
        return this;
    }

    public IUserBaseBuilder WithEmail(string email)
    {
        _userBase.Email = email;
        return this;
    }

    public IUserBaseBuilder BuildCompany()
    {
        _userBase.Company = string.Empty;
        return this;
    }

    public IUserBaseBuilder WithCompany(string company)
    {
        _userBase.Company = company;
        return this;
    }

    public IUserBaseBuilder BuildTitle()
    {
        _userBase.Title = string.Empty;
        return this;
    }

    public IUserBaseBuilder WithTitle(string title)
    {
        _userBase.Title = title;
        return this;
    }

    public IUserBaseBuilder BuildLang()
    {
        _userBase.Lang = "da";
        return this;
    }

    public IUserBaseBuilder WithLang(string lang)
    {
        _userBase.Lang = lang;
        return this;
    }

    public IUserBaseBuilder BuildLastLoginTime()
    {
        _userBase.LastLoginTime = DateTime.Now;
        return this;
    }

    public IUserBaseBuilder WithLastLoginTime(DateTime lastLogin)
    {
        _userBase.LastLoginTime = lastLogin;
        return this;
    }

    public IUserBaseBuilder BuildUseTwoFactor()
    {
        _userBase.UseTwoFactor = true;
        return this;
    }

    public IUserBaseBuilder WithUseTwoFactor(bool twoFactor)
    {
        _userBase.UseTwoFactor = twoFactor;
        return this;
    }
}