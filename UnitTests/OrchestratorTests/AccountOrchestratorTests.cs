using AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator;
using AdminDashboard.Exceptions;
using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Domain;
using AdminDashboard.Models.JsonRequests;
using AdminDashboard.Models.Plans;
using AdminDashboard.Repositories.AccountRepository;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.OrchestratorTests
{
    public class AccountOrchestratorTests
    {
        private readonly Guid UserId;
        private readonly Account Account;
        private Mock<IAccountRepository> Repository;
        private IAccountOrchestrator Orchestrator;

        public AccountOrchestratorTests()
        {
            UserId = Guid.NewGuid();
            Account = GetAccount(UserId);
            Repository = GetRepositoryMock(Account);
            Orchestrator = new AccountOrchestrator(Repository.Object);
        }

        [Fact]
        public async Task GetAccountBy__ReturnsAccount()
        {
            var account = await Orchestrator.GetAccountBy(Account.Id);

            Assert.Single(account.Users);
            Assert.Equal(Account.Id, account.Id);
            Assert.Equal(Plan.Enterprise, account.CurrentPlan);
        }

        [Fact]
        public async Task GetAccountBy_WhenDoesntExist_ThrowsException()
        {
            Repository = new Mock<IAccountRepository>();
            Orchestrator = new AccountOrchestrator(Repository.Object);

            await Assert.ThrowsAsync<AccountNotFoundException>(
                async () => await Orchestrator.GetAccountBy(Account.Id));
        }

        [Fact]
        public async Task GetAccountUsers_WhenAccountDoesntExist_ThrowsException()
        {
            Repository = new Mock<IAccountRepository>();
            Orchestrator = new AccountOrchestrator(Repository.Object);

            await Assert.ThrowsAsync<AccountNotFoundException>(
                async () => await Orchestrator.GetAccountUsers(Account.Id));
        }

        [Fact]
        public async Task UpgradePlan_WhenAccountDoesntExist_ThrowsException()
        {
            Repository = new Mock<IAccountRepository>();
            Orchestrator = new AccountOrchestrator(Repository.Object);

            await Assert.ThrowsAsync<AccountNotFoundException>(
                async () => await Orchestrator.UpgradePlan(Account.Id, null));
        }

        [Fact]
        public async Task UpgradePlan_WhenProposedPlanSameAsCurrentPlan_ThrowsException()
        {
            var request = new UpgradePlanRequest() { Plan = Account.CurrentPlan.GetPlanType() };

            await Assert.ThrowsAsync<InvalidUpgradePlanException>(
                async () => await Orchestrator.UpgradePlan(Account.Id, request));
        }

        [Fact]
        public async Task UpgradePlan_WhenProposedPlanLesserThanCurrentPlan_ThrowsException()
        {
            var request = new UpgradePlanRequest() { Plan = (Plan)1 };

            await Assert.ThrowsAsync<InvalidUpgradePlanException>(
                async () => await Orchestrator.UpgradePlan(Account.Id, request));
        }

        [Fact]
        public async Task RegisterUserToAccount_WhenAccountDoesntExist_ThrowsException()
        {
            Repository = new Mock<IAccountRepository>();
            Orchestrator = new AccountOrchestrator(Repository.Object);

            await Assert.ThrowsAsync<AccountNotFoundException>(
                async () => await Orchestrator.RegisterUserToAccount(Account.Id, Guid.NewGuid()));
        }

        [Fact]
        public async Task RegisterUserToAccount_WhenUserAlreadyExistsOnAccount_ThrowsException()
        {
            await Assert.ThrowsAsync<DuplicateUserOnAccountException>(
                async () => await Orchestrator.RegisterUserToAccount(Account.Id, UserId));
        }

        [Fact]
        public async Task RegisterUserToAccount_WhenUserLimitReached_ThrowsException()
        {
            var account = GetStartupAccountWithNumberOfUsers(new StartupPlan().MaxNumberOfUsersAllowed);
            Repository = GetRepositoryMock(account);
            Orchestrator = new AccountOrchestrator(Repository.Object);

            await Assert.ThrowsAsync<UserLimitExceededException>(
                async () => await Orchestrator.RegisterUserToAccount(account.Id, UserId));
        }

        private static Mock<IAccountRepository> GetRepositoryMock(Account account)
        {
            var repository = new Mock<IAccountRepository>();

            repository.Setup(r => r.GetAccountBy(account.Id))
                .ReturnsAsync(account);

            return repository;
        }

        private static Account GetAccount(Guid userId)
        {
            var planType = new EnterprisePlan();
            var account = new Account(planType);

            account.RegisterUser(userId);

            return account;
        }

        private static Account GetStartupAccountWithNumberOfUsers(int times = 1)
        {
            var planType = new StartupPlan();
            var account = new Account(planType);

            for (int i = 0; i < times; i++)
            {
                Guid userId = Guid.NewGuid();

                account.RegisterUser(userId);
            }

            return account;
        }
    }
}