using App.Betterboard.Api.Model.Validators;
using App.Betterboard.Api.Services.Interfaces;
using App.BetterBoard.Api.Services.Transaction.Interfaces;
using App.BetterBoard.Api.Services.Transaction;
using App.Betterboard.Models;
using MongoDB.Bson;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using App.BetterBoard.Api;
using App.BetterBoard.Tests.Unit.Services;
using App.Betterboard;
using BetterBoard.Core.Data.Interfaces;
using IBoardRepository = BetterBoard.Core.Data.Interfaces.IBoardRepository;
using App.BetterBoard.Api.Exceptions.Transaction;

namespace App.BetterBoard.Tests.Unit.Services
{
    public class TransactionServiceRestrictCallerTests
    {
        private readonly IBoardRepository _boardRepository = Substitute.For<IBoardRepository>();
        private readonly IBoardService _boardService = Substitute.For<IBoardService>();
        private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
        private readonly ICompanyService _companyService = Substitute.For<ICompanyService>();
        private readonly IDataroomRepository _dataroomRepository = Substitute.For<IDataroomRepository>();
        private readonly ApplicationUserManager _userManager;
        private readonly IUserService _userService = Substitute.For<IUserService>();
        private readonly IMemberValidator _memberValidator = Substitute.For<IMemberValidator>();
        private readonly ITransactionService _sut;

        public TransactionServiceRestrictCallerTests()
        {
            var userStore = Substitute.For<IUserStore<ApplicationUser>>();
            _userManager = Substitute.For<ApplicationUserManager>(userStore);

            _sut = new TransactionService(_companyService,
                                          _boardService,
                                          _memberValidator,
                                          _boardRepository,
                                          _userManager,
                                          _userService,
                                          _categoryRepository,
                                          _dataroomRepository);
        }

        [Fact]
        public async Task RestrictCallerAttribute_ShouldThrow_WhenCalledFromUnauthorizedClass()
        {
            var exception = await _sut.Seed(null).ShouldThrowAsync<UnauthorizedInstanceCallException>();
            exception.Message.ShouldBe("This method may only be called by a select few classes: " 
                + string.Join(" ", new string[] { nameof(TestTransactionController), nameof(TransactionServiceTests) }));
        }
    }
}
