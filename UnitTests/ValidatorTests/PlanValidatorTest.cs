using AdminDashboard.Main.Enumerations;
using AdminDashboard.Validators.Plans;
using Xunit;

namespace UnitTests.ValidatorTests
{
    public class PlanValidatorTest
    {
        private readonly IPlanValidator Validator;

        public PlanValidatorTest()
        {
            Validator = new PlanValidator();
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
    }
}