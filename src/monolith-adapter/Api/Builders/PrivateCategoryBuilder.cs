using App.Betterboard.Api.Model;
using MongoDB.Bson;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;
using System.Collections.Generic;
using App.Betterboard.Api.Services;
using BetterBoard.Shared.Model;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Api.Builders
{
    public class PrivateCategoryBuilder
    {
        private readonly ObjectId _boardId;
        private readonly ObjectId _userId;

        public PrivateCategoryBuilder(ObjectId withBoardId, ObjectId withUserId)
        {
            _boardId = withBoardId;
            _userId = withUserId;
        }

        public Category Build()
        {
            return new Category
            {
                BoardId = _boardId,
                UserId = _userId,
                Title = CONSTANTS.FOLDERNAME_MY_PRIVATE_DOCUMENTS,
                IconClass = CONSTANTS.FOLDERNAME_MY_PRIVATE_DOCUMENTS,
                Folders = new List<Folder>(),
                Documents = new List<Document>()
            };
        }
    }
}