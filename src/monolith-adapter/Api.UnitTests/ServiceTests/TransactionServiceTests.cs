using App.Betterboard.Api.Services.Interfaces;
using App.Betterboard;
using AutoMapper;
using NSubstitute;
using System.Collections;
using App.Betterboard.Api.Model;
using MongoDB.Bson;
using Microsoft.AspNet.Identity;
using App.Betterboard.Models;
using App.Betterboard.Api.Model.Validators;
using FluentValidation.Results;
using App.BetterBoard.Api.Exceptions;
using NSubstitute.ReceivedExtensions;
using App.BetterBoard.Tests.Unit.Builders.Implementation;
using NSubstitute.ReturnsExtensions;
using App.Betterboard.Api.Exceptions;
using App.BetterBoard.Api.Services.Transaction.Interfaces;
using App.BetterBoard.Api.Services.Transaction;
using BetterBoard.Core.Data.Interfaces;
using BetterBoard.Core.Model;


namespace App.BetterBoard.Tests.Unit.Services;

public class TransactionServiceTests
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

    private readonly string _defaultCompanyId = ObjectId.GenerateNewId().ToString();
    private readonly string _defaultBoardId = ObjectId.GenerateNewId().ToString();
    private readonly Guid _defaultIdentifier = Guid.NewGuid();

    public TransactionServiceTests()
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

    [Theory]
    [ClassData(typeof(BoardServiceTests_NullOrDefaultArguments_TestData))]
    public async Task AddPresetMemberToBoard_ShouldThrow_WhenArgumentsArePassedAsNullOrDefault(
        string companyId,
        string boardId,
        Member member,
        Guid identifier)
    {
        _ = await _sut.AddPresetMemberToBoard(companyId, boardId, member, identifier).ShouldThrowAsync<Exception>();
    }

    [Fact]
    public async Task AddPresetMemberToBoard_ShouldThrow_WhenMemberArgumentDoesNotValidate()
    {
        // Arrange
        // ValidationResult type's property 'IsValid' defaults to false, if any errors are passed in as constructor arguments.
        _memberValidator.Validate(Arg.Any<Member>())
            .ReturnsForAnyArgs(new ValidationResult(new[] { new ValidationFailure("Test Property", "Test Error Message") }));
        var testMember = new Member();

        // Act & Assert
        await _sut.AddPresetMemberToBoard(_defaultCompanyId,
                                          _defaultBoardId,
                                          testMember,
                                          _defaultIdentifier)
            .ShouldThrowAsync<InvalidMemberException>();
        _memberValidator.ReceivedWithAnyArgs(1).Validate(Arg.Any<Member>());
    }

    [Fact]
    public async Task AddPresetMemberToBoard_ShouldThrow_WhenBoardRepoReturnNull()
    {
        // Arrange
        _memberValidator.Validate(Arg.Any<Member>())
            .ReturnsForAnyArgs(new ValidationResult());
        _boardRepository.GetAsync(Arg.Any<string>())
            .ReturnsNullForAnyArgs<Board>();
        var testUserBase = new DefaultUserBaseBuilder().Build();
        var testMember = new DefaultMemberBuilder().Build(testUserBase);

        // Act & Assert
        var exception = await _sut.AddPresetMemberToBoard(_defaultCompanyId,
                                          _defaultBoardId,
                                          testMember,
                                          _defaultIdentifier)
            .ShouldThrowAsync<BoardNotFoundException>();

        // Additional Assertion
        exception.Message.ShouldBe("No board found with the id: " + _defaultBoardId);
        await _boardRepository.ReceivedWithAnyArgs(1)
            .GetAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task AddPresetMemberToBoard_ShowThrow_WhenUserIsCreatedWithInvalidEmail()
    {
        // Arrange
        _memberValidator.Validate(Arg.Any<Member>())
            .ReturnsForAnyArgs(new ValidationResult());
        var testMember = MockRepos();
        _userManager.FindByEmailAsync(Arg.Any<string>())
            .ReturnsForAnyArgs<Task<ApplicationUser>>(
            (info) => Task.FromResult<ApplicationUser>(null));
        _userManager.CreateAsync(Arg.Any<ApplicationUser>())
            .ReturnsForAnyArgs<Task<IdentityResult>>(
            Task.FromResult<IdentityResult>(
                IdentityResult.Success));

        // Act & Assertion
        var exception = await _sut.AddPresetMemberToBoard(_defaultCompanyId,
            _defaultBoardId,
            testMember,
            _defaultIdentifier)
            .ShouldThrowAsync<NullReferenceException>();

        // Additional Assert
        exception.Message.ShouldBe("Could not find new user by email, even after creating.");
        await _userManager.ReceivedWithAnyArgs(1)
            .CreateAsync(Arg.Any<ApplicationUser>());
        await _userManager.ReceivedWithAnyArgs(2)
            .FindByEmailAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task AddPresetMemberToBoard_ShowThrow_WhenUserAlreadyExistsOnBoard()
    {
        // Arrange
        _memberValidator.Validate(Arg.Any<Member>())
            .ReturnsForAnyArgs(new ValidationResult());
        var testMember = MockRepos();
        var testAppUser = new ApplicationUserBuilder(testMember,
                                                     _defaultIdentifier,
                                                     _userManager).Build();
        _userManager.FindByEmailAsync(Arg.Any<string>())
            .ReturnsForAnyArgs<Task<ApplicationUser>>(
            (info) => Task.FromResult<ApplicationUser>(testAppUser));
        _userService.UndeleteUser(Arg.Any<string>())
            .ReturnsForAnyArgs<Task>(Task.CompletedTask);
        _userService.GetAsync(Arg.Any<string>())
            .ReturnsForAnyArgs<Task<User>>(
            Task.FromResult<User>(new User()));

        // Act & Assert
        var exception = await _sut.AddPresetMemberToBoard(_defaultCompanyId,
            _defaultBoardId,
            testMember,
            _defaultIdentifier)
            .ShouldThrowAsync<Exception>();

        // Additional Assertion
        exception.Message.ShouldBe("User already exists on the board");
        await _userService.ReceivedWithAnyArgs(0)
            .GetAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task AddPresetMemberToBoard_ShouldSucceed_WhenCalledWithValidArguments()
    {
        // Arrange
        _memberValidator.Validate(Arg.Any<Member>())
            .ReturnsForAnyArgs(new ValidationResult());
        var testUserBase = new DefaultUserBaseBuilder().Build();
        var testMember = new DefaultMemberBuilder().Build(testUserBase);
        var testBoard = new DefaultBoardBuilder().Build();
        _boardRepository.GetAsync(Arg.Any<string>())
            .ReturnsForAnyArgs<Task<Board>>(Task.FromResult<Board>(testBoard));
        var testAppUser = new ApplicationUserBuilder(testMember,
                                                     _defaultIdentifier,
                                                     _userManager).Build();
        _userManager.FindByEmailAsync(Arg.Any<string>())
            .ReturnsForAnyArgs<Task<ApplicationUser>>(
            (info) => Task.FromResult<ApplicationUser>(testAppUser));
        _userService.UndeleteUser(Arg.Any<string>())
            .ReturnsForAnyArgs<Task>(Task.CompletedTask);
        var testUser = new DefaultUserBuilder(testUserBase).Build();
        _userService.GetAsync(Arg.Any<string>())
            .ReturnsForAnyArgs<Task<User>>(
            Task.FromResult<User>(testUser));
        _categoryRepository.Create(Arg.Any<Category>())
            .ReturnsForAnyArgs(Task.FromResult(new Category()));
        _userService.UpdateUserAsync(
            Arg.Any<UpdateUserProfileDto>(),
            Arg.Any<string>())
            .ReturnsForAnyArgs<Task<User>>(Task.FromResult(testUser));
        _boardRepository.AddMember(
            Arg.Any<string>(),
            Arg.Any<Member>())
            .ReturnsForAnyArgs(
            Task.FromResult<Board>(
                BoardRepoMock(
                    testBoard,
                    testMember)));

        // Act
        var board = await _sut.AddPresetMemberToBoard(_defaultCompanyId,
            _defaultBoardId,
            testMember,
            _defaultIdentifier);

        // Assert
        await _boardRepository.ReceivedWithAnyArgs(1)
            .AddMember(Arg.Any<string>(), Arg.Any<Member>());
        Assert.Contains(testMember, board.Members);
    }

    private Board BoardRepoMock(in Board board, in Member member)
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Board, Board>());
        var mapper = config.CreateMapper();
        var newBoard = mapper.Map<Board>(board);
        newBoard.Members.Add(member);
        return newBoard;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The test member instantiated and added to the test board</returns>
    private Member MockRepos()
    {
        var testUserBase = new DefaultUserBaseBuilder().Build();
        var testMember = new DefaultMemberBuilder().Build(testUserBase);
        var testBoard = new DefaultBoardBuilder()
            .Build();
        testBoard.Members.Add(testMember);
        _boardRepository.GetAsync(Arg.Any<string>())
            .ReturnsForAnyArgs<Task<Board>>(Task.FromResult(testBoard));
        return testMember;
    }

    class BoardServiceTests_NullOrDefaultArguments_TestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                ObjectId.GenerateNewId(),
                new Member(),
                Guid.NewGuid(),
            };
            yield return new object[]
            {
                ObjectId.GenerateNewId(),
                null,
                new Member(),
                Guid.NewGuid(),
            };
            yield return new object[]
            {
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId(),
                null,
                Guid.NewGuid(),
            };
            yield return new object[]
            {
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId(),
                new Member(),
                default(Guid),
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}