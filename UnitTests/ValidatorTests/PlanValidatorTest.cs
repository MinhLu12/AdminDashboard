using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Domain;
using AdminDashboard.Models.Plans;
using AdminDashboard.Repositories.AccountRepository;
using AdminDashboard.Validators.Plans;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ValidatorTests
{
    public class PlanValidatorTest
    {
        private Guid AccountId = Guid.NewGuid();
        private PlanType PlanType = new StartupPlan();
        private readonly IAccountRepository AccountRepository;
        private readonly IPlanValidator Validator;

        public PlanValidatorTest()
        {
            AccountRepository = GetAccountRepositoryWith(PlanType);
            Validator = new PlanValidator(AccountRepository);
        }

        [Fact]
        public void IsPlanValid__ReturnsTrue()
        {
            Assert.True(Validator.IsPlanValid(Plan.Startup));
        }

        [Fact]
        public void IsPlanValid_WhenPlanNotInEnum_ReturnsFalse()
        {
            Assert.False(Validator.IsPlanValid((Plan)100));
        }

        [Fact]
        public async Task IsValidUpgradeTransition_WhenStartupToEnterprise_ReturnsTrue()
        {
            bool isValidTransition = await Validator.IsValidUpgradeTransition(AccountId, Plan.Enterprise);

            Assert.True(isValidTransition);
        }

        [Fact]
        public async Task IsValidUpgradeTransition_WhenEnterpriseToStartup_ReturnsFalse()
        {
            PlanType = new EnterprisePlan();
            PlanValidator validator = GetValidator();

            bool isValidTransition = await validator.IsValidUpgradeTransition(AccountId, Plan.Enterprise);

            Assert.False(isValidTransition);
        }

        private PlanValidator GetValidator()
        {
            var accountRepository = GetAccountRepositoryWith(PlanType);
            var validator = new PlanValidator(accountRepository);
            return validator;
        }

        [Fact]
        public async Task IsValidUpgradeTransition_WhenNoDifferenceInPlans_ReturnsFalse()
        {
            bool isValidTransition = await Validator.IsValidUpgradeTransition(AccountId, Plan.Startup);

            Assert.False(isValidTransition);
        }

        private IAccountRepository GetAccountRepositoryWith(PlanType plan)
        {
            var repository = new Mock<IAccountRepository>();
            repository.Setup(r => r.GetAccountBy(It.IsAny<Guid>()))
                .Returns(Task.FromResult(GetAccountWith(plan)));

            return repository.Object;
        }

        private Account GetAccountWith(PlanType plan)
        {
            return new Account(plan);
        }
    }
}
