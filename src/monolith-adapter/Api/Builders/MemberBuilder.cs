using App.Betterboard.Api.Model;
using App.Betterboard.Api.Services;
using AutoMapper;
using BetterBoard.Core.Model;
using System;

namespace App.BetterBoard.Api.Builders
{
    public class MemberBuilder
    {
        private readonly UserBase _userBase;

        public MemberBuilder(in UserBase userBase)
        {
            _userBase = userBase;
        }

        public Member Build()
        {
            var config = new MapperConfiguration(cfg => 
                cfg.CreateMap<UserBase, Member>());
            var mapper = config.CreateMapper();
            var member = mapper.Map<Member>(_userBase);
            member.Role = CONSTANTS.ROLE_BOARDMEMBER;
            member.AccessLevel = CONSTANTS.ACL_USERLVL2;
            member.Assignment = string.Empty;
            member.AddedToBoard = DateTime.Now;
            return member;
        }
    }
}