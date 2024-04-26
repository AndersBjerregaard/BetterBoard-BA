using BetterBoard.Core.Model;
using App.BetterBoard.Api.Exceptions.Transaction;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace App.BetterBoard.Api.Services.Transaction.Interfaces
{
    public interface ITransactionService
    {
        /// <exception cref="InvalidTransactionException"></exception>
        Task Seed(TestRunIdentifier runIdentifier);

        Task<Board> AddPresetMemberToBoard(string companyId, string boardId, Member member, Guid identifier);

        Task<ObjectId> CreateCategoryAsync(ObjectId companyId, ObjectId boardId, Category category);

        Task<User> CreateUserAsync(User user);

        Task<Dataroom> CreateDataroomAsync(CreateDataroomDto dataroomDto, ObjectId companyId, ObjectId boardId);

        /// <exception cref="InvalidTransactionException"></exception>
        Task CleanUp(TestRunIdentifier runIdentifier);
    }
}
