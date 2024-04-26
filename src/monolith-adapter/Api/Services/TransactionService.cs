using App.BetterBoard.Api.Builders;
using App.Betterboard.Api.Model;
using App.BetterBoard.Api.Services.Transaction.Interfaces;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;
using App.BetterBoard.Api.Model;
using App.Betterboard.Api.Services.Interfaces;
using System.Linq;
using App.BetterBoard.Api.Exceptions.Transaction;
using App.Betterboard.Api.Exceptions;
using App.BetterBoard.Api.Exceptions;
using App.Betterboard.Api.Extensions;
using App.Betterboard.Models;
using App.Betterboard.Api.Model.Validators;
using App.Betterboard;
using App.BetterBoard.Api.Attributes;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using BetterBoard.Core.Data.Interfaces;
using BetterBoard.Core.Model;

namespace App.BetterBoard.Api.Services.Transaction
{
    public class TransactionService : ITransactionService
    {
        private const string COMPANY_NAME_PREFIX = "E2E Test Company - ";

        private readonly ICompanyService _companyService;
        private readonly IBoardService _boardService;
        private readonly IMemberValidator _memberValidator;
        private readonly IBoardRepository _boardRepository;
        private readonly ApplicationUserManager _userManager;
        private readonly IUserService _userService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDataroomRepository _dataroomRepository;

        public IEnumerable<string> AllowedCallers { get; private set; } 
            = new string[] { nameof(TestTransactionController), "TransactionServiceTests" };

        public TransactionService(ICompanyService companyService,
                                  IBoardService boardService,
                                  IMemberValidator memberValidator,
                                  IBoardRepository boardRepository,
                                  ApplicationUserManager userManager,
                                  IUserService userService,
                                  ICategoryRepository categoryRepository,
                                  IDataroomRepository dataroomRepository)
        {
            _companyService = companyService;
            _boardService = boardService;
            _memberValidator = memberValidator;
            _boardRepository = boardRepository;
            _userManager = userManager;
            _userService = userService;
            _categoryRepository = categoryRepository;
            _dataroomRepository = dataroomRepository;
        }

        /// <exception cref="InvalidTransactionException"></exception>
        public async Task Seed(TestRunIdentifier runIdentifier)
        {
            ValidateCaller();

            ValidateInput(runIdentifier, out Guid identifier, out ObjectId companyId);

            // Create Company
            var existingCompany = await _companyService.SearchAsync(
                new CompanySearchDto { CompanyName = COMPANY_NAME_PREFIX + runIdentifier });

            if (existingCompany.Any())
            {
                throw new InvalidTransactionException($"A company with the name '{COMPANY_NAME_PREFIX + runIdentifier.Id}' already exists.");
            }

            var company = new Company
            {
                _id = companyId,
                CompanyName = COMPANY_NAME_PREFIX + identifier.ToString(),
                Address = string.Empty,
                Zip = string.Empty,
                City = string.Empty,
                VAT = string.Empty,
                Licenses = 50,
                Contact = new Contact(),
                DefaultLang = "da",
                Status = CompanyStatus.DA_PREMIUM,
                Partner = 1,
                SignaturesPerYear = 50
            };

            company = await _companyService.Create(company, ObjectId.Empty.ToString());

            // Create Board
            var board = new Board { BoardName = "E2E Test Board - " + identifier.ToString("N") };

            board = await _boardService.CreateBoard(companyId.ToString(), board, ObjectId.Empty.ToString());

            var boardId = board._id;

            // Create Dataroom

            // Create Users
            var userBase = new UserBase
            {
                _id = ObjectId.GenerateNewId(),
                Created = DateTime.Now,
                Name = "Test User - " + identifier.ToString("N"),
                CountryCode = "+45",
                Phone = "12345678",
                Email = identifier.ToString("N") + "@mail.dk",
                Company = company.CompanyName,
                Title = "Board member",
                Lang = "da",
                LastLoginTime = DateTime.Now,
                UseTwoFactor = false,
            };

            var member = new MemberBuilder(userBase).Build();

           _ = await AddPresetMemberToBoard(companyId.ToString(),
                                                       boardId.ToString(),
                                                       member,
                                                       identifier);
        }

        /// <param name="identifier">Will set the password of the created Member. With the N format of the Guid.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="InvalidMemberException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<Board> AddPresetMemberToBoard(string companyId, string boardId, Member member, Guid identifier)
        {
            ValidateCaller();

            if (companyId == null)
                throw new ArgumentNullException(nameof(companyId));
            if (boardId == null)
                throw new ArgumentNullException(nameof(boardId));
            if (member == null)
                throw new ArgumentNullException(nameof(member));
            if (identifier == default)
                throw new Exception($"Parameter value of {nameof(identifier)} must not be default");

            var validationResult = _memberValidator.Validate(member);

            if (!validationResult.IsValid)
            {
                throw new InvalidMemberException(
                    "Member from request body did not validate successfully: " + String.Join("\n", validationResult.Errors));
            }

            member.Email = member.Email.Trim();
            member.Name = member.Name.Trim();

            var board = await _boardRepository.GetAsync(boardId)
                ?? throw new BoardNotFoundException($"No board found with the id: {boardId}");

            if (String.IsNullOrEmpty(member.Lang))
                member.Lang = board.Company.DefaultLang;

            var newUserInfo = new UserBuilder(member).Build();

            var user = await _userManager.FindByEmailAsync(member.Email);

            User userInfo = null; // Will be populated instantiated differently, depending on the case below

            // Create user if user doesn't exist
            if (user == null)
            {
                var result = await _userManager.CreateAsync(new ApplicationUser
                {
                    EmailAddress = member.Email,
                    UserName = member.Email,
                    PhoneNumber = member.Phone.TrimWhiteSpaces(),
                    PasswordHash = _userManager.PasswordHasher.HashPassword(identifier.ToString("N")),
                    TwoFactorEnabled = false,
                });

                if (!result.Succeeded)
                    throw new Exception(
                        "Failed to create new user: " + String.Join("\n", result.Errors));

                var newUser = await _userManager.FindByEmailAsync(member.Email)
                    ?? throw new NullReferenceException("Could not find new user by email, even after creating.");

                newUserInfo._id = ObjectId.Parse(newUser.Id);
                member._id = ObjectId.Parse(newUser.Id);

                userInfo = await CreateUserAsync(newUserInfo);
            }
            else
            {
                await _userService.UndeleteUser(user.Id);

                newUserInfo._id = ObjectId.Parse(user.Id);
                member._id = ObjectId.Parse(user.Id);

                if (board.Members.Any(m => m._id == member._id))
                    throw new Exception("User already exists on the board");

                userInfo = await _userService.GetAsync(user.Id)
                    ?? await CreateUserAsync(newUserInfo);
            }

            member.Created = userInfo.Created;
            member.CreatedBy = userInfo.CreatedBy;

            var privateCategory = new PrivateCategoryBuilder(
                ObjectId.Parse(boardId),
                member._id)
                .Build();

            await _categoryRepository.Create(privateCategory);

            return await _boardRepository.AddMember(boardId, member);
        }

        public async Task<ObjectId> CreateCategoryAsync(ObjectId companyId, ObjectId boardId, Category category)
        {
            ValidateCaller();

            category.BoardId = boardId;
            category.Created = DateTime.Now;
            var newCategory = await _categoryRepository.Create(category);
            return newCategory._id;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            ValidateCaller();

            if (string.IsNullOrEmpty(user.Lang))
            {
                user.Lang = "en";
            }

            user.Phone = user.Phone.Replace("+", "");
            user.Phone = user.Phone.Replace(" ", "");
            user.Phone = user.Phone.TrimWhiteSpaces();
            user.Name = user.Name.Trim();

            return await _userRepository.Create(user);
        }

        public async Task<Dataroom> CreateDataroomAsync(CreateDataroomDto dataroomDto, ObjectId companyId, ObjectId boardId)
        {
            ValidateCaller();

            var dataroomCategory = new DataroomCategoryBuilder(
                dataroomDto,
                boardId)
                .Build();

            var categoryId = await CreateCategoryAsync(
                companyId,
                boardId,
                dataroomCategory);

            var dataroom = new DataroomBuilder(
                boardId,
                companyId,
                categoryId,
                dataroomDto).Build();

            return await _dataroomRepository.Create(dataroom);
        }

        /// <exception cref="InvalidTransactionException"></exception>
        public async Task CleanUp(TestRunIdentifier runIdentifier)
        {
            ValidateCaller();

            ValidateInput(runIdentifier, out Guid identifier, out ObjectId companyId);

            await _companyService.ForceDelete(companyId.ToString(),
                                              identifier);
        }

        /// <exception cref="InvalidTransactionException"></exception>
        private void ValidateInput(TestRunIdentifier runIdentifier, out Guid identifier, out ObjectId companyId)
        {
            if (runIdentifier == null)
            {
                throw new InvalidTransactionException($"Argument {nameof(runIdentifier)} cannot be null.", new ArgumentNullException(nameof(runIdentifier)));
            }
            if (string.IsNullOrEmpty(runIdentifier.Id))
            {
                throw new InvalidTransactionException($"Property {nameof(runIdentifier.Id)} cannot be null or empty.", new ArgumentNullException(nameof(runIdentifier.Id)));
            }
            if (string.IsNullOrEmpty(runIdentifier.CompanyId))
            {
                throw new InvalidTransactionException($"Property {nameof(runIdentifier.CompanyId)} cannot be null or empty.", new ArgumentNullException(nameof(runIdentifier.CompanyId)));
            }

            var identifierParse = Guid.TryParseExact(runIdentifier.Id, "N", out identifier);
            if (!identifierParse)
            {
                throw new InvalidTransactionException($"Could not parse {nameof(runIdentifier.Id)} as a Guid.");
            }
            var companyIdParse = ObjectId.TryParse(runIdentifier.CompanyId, out companyId);
            if (!companyIdParse)
            {
                throw new InvalidTransactionException($"Could not parse {nameof(runIdentifier.CompanyId)} as an ObjectId.");
            }
        }

        /// <summary>
        /// Uses reflection to get the classnames in the stacktrace of the given method call.
        /// And compare the classnames to the list of classnames that are allowed to call this class' methods.
        /// </summary>
        /// <exception cref="UnauthorizedInstanceCallException"></exception>
        private void ValidateCaller()
        {
            var stackTrace = new StackTrace().GetFrames();
            for (int i = 0; i < stackTrace.Length; i++)
            {
                if (AllowedCallers.Contains(stackTrace[i].GetMethod().DeclaringType.Name))
                    return;
            }
            throw new UnauthorizedInstanceCallException("This method may only be called by a select few classes: " + string.Join(" ", AllowedCallers));
        }
    }
}