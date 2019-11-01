using AdminDashboard.Models.Plans;
using Xunit;

namespace UnitTests.ModelTests
{
    public class PlanTests
    {
        [Fact]
        public void ConstructStartupPlan__ReturnsCorrectPlanFields()
        {
            var plan = new StartupPlan();

            Assert.Equal(100, plan.MaxNumberOfUsersAllowed);
            Assert.Equal(100.00F, plan.PricePerMonth);
        }

        [Fact]
        public void ConstructEnterprisePlan__ReturnsCorrectPlanFields()
        {
            var plan = new EnterprisePlan();

            Assert.Equal(1000, plan.MaxNumberOfUsersAllowed);
            Assert.Equal(1000.00F, plan.PricePerMonth);
        }
    }
}
