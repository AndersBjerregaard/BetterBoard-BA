using App.Betterboard.Api.Model;
using BetterBoard.Core.Model;
using BetterBoard.Shared.Model;
using MongoDB.Bson;
using System.Collections.Generic;

namespace App.BetterBoard.Api.Builders
{
    public class DataroomCategoryBuilder
    {
        private readonly ObjectId _boardId;
        private readonly CreateDataroomDto _createDataroomDto;

        public DataroomCategoryBuilder(CreateDataroomDto createDataroomDto,
                                       ObjectId boardId)
        {
            _createDataroomDto = createDataroomDto;
            _boardId = boardId;
        }

        public Category Build()
        {
            return new Category
            {
                BoardId = _boardId,
                Title = _createDataroomDto.Title,
                Folders = new List<Folder>(),
                Documents = new List<Document>(),
                Type = CategoryType.Dataroom,
                Description = _createDataroomDto.SubTitle,
                Accesslist = null,
            };
        }
    }
}