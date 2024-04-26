using App.Betterboard.Api.Model;
using BetterBoard.Core.Model;
using MongoDB.Bson;
using System;

namespace App.BetterBoard.Api.Builders
{
    public class DataroomBuilder
    {
        private readonly ObjectId _boardId;
        private readonly ObjectId _companyId;
        private readonly ObjectId _categoryId;
        private readonly CreateDataroomDto _createDataroomDto;

        public DataroomBuilder(ObjectId boardId, ObjectId companyId, ObjectId categoryId, CreateDataroomDto createDataroomDto)
        {
            _boardId = boardId;
            _companyId = companyId;
            _categoryId = categoryId;
            _createDataroomDto = createDataroomDto;
        }

        public Dataroom Build()
        {
            return new Dataroom
            {
                _id = ObjectId.GenerateNewId(),
                BoardId = _boardId,
                CompanyId = _companyId,
                Created = DateTime.Now,
                CategoryId = _categoryId,
                Title = _createDataroomDto.Title,
                SubTitle = _createDataroomDto.SubTitle,
                EnableRequestList = _createDataroomDto.EnableRequestList,
                EnableQA = _createDataroomDto.EnableQA,
                Deleted = false,
                InternalUsersRole = _createDataroomDto.InternalUsersRole,
                ExternalUsersRole = _createDataroomDto.ExternalUsersRole,
            };
        }
    }
}