using App.Betterboard.Api.Model;
using MongoDB.Bson;
using System.Collections.Generic;
using System;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Api.Builders
{
    /// <summary>
    /// Builds a User instance by mapping correlating properties from a Member instance.
    /// </summary>
    public class UserBuilder
    {
        private readonly Member _member;

        public UserBuilder(Member member)
        {
            _member = member;
        }

        public User Build()
        {
            return new User
            {
                Name = _member.Name,
                Email = _member.Email,
                _id = _member._id,
                CountryCode = _member.CountryCode,
                EulaAccepted = DateTime.Now,
                GDPRAccepted = DateTime.Now,
                Phone = _member.Phone,
                ReadList = new List<ReadDocument>(),
                Created = DateTime.Now,
                Categories = new List<Category>(),
                identity = _member._id,
                Permission = true,
                Picture = _member.Picture,
                UserHasToChangePassword = false,
                Lang = _member.Lang,
                Company = _member.Company,
                Title = _member.Title,
                UseTwoFactor = _member.UseTwoFactor,
            };
        }
    }
}