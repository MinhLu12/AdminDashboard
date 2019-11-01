using AdminDashboard.Models.Domain;
using AdminDashboard.Models.Plans;
using AdminDashboard.Repositories.AccountRepository;
using AdminDashboard.Validators.Accounts;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ValidatorTests
{
    public class AccountValidatorTests
    {
        private readonly IAccountValidator Validator;
        private readonly Guid UserId = Guid.NewGuid();
        private readonly Account Account;

        public AccountValidatorTests()
        {
            Account = GetAccountWith(UserId);
            Validator = GetValidator();
        }

        [Fact]
        public async Task DoesAccountAlreadyHaveUser__ReturnsTrue()
        {
            Guid accountId = Guid.NewGuid();
            bool hasUser = await Validator.DoesAccountAlreadyHaveUser(accountId, UserId);

            Assert.True(hasUser);
        }

        [Fact]
        public async Task DoesAccountAlreadyHaveUser_WhenUserExists_ReturnsFalse()
        {
            Guid accountId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            bool hasUser = await Validator.DoesAccountAlreadyHaveUser(accountId, userId);

            Assert.False(hasUser);
        }

        private AccountValidator GetValidator()
        {
            var repository = GetAccountRepository();
            return new AccountValidator(repository);
        }

        private IAccountRepository GetAccountRepository()
        {
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.GetAccountBy(It.IsAny<Guid>()))
                .Returns(Task.FromResult(Account));

            return repository.Object;
        }

        private static Account GetAccountWith(Guid userId)
        {
            var a = new Account(new EnterprisePlan());

            a.RegisterUser(userId);

            return a;
        }
    }
}
