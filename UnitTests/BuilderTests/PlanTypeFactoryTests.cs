using AdminDashboard.Main.Enumerations;
using AdminDashboard.Main.Factories;
using AdminDashboard.Models.Plans;
using Xunit;

namespace UnitTests.BuilderTests
{
    public class PlanTypeFactoryTests
    {
        private readonly IPlanTypeFactory Factory;

        public PlanTypeFactoryTests()
        {
            Factory = new PlanTypeFactory();
        }

        [Fact]
        public void BuildFrom_ReturnStartupPlan()
        {
            var planType = Factory.BuildFrom(Plan.Startup);

            Assert.IsType<StartupPlan>(planType);
        }

        [Fact]
        public void BuildFrom_ReturnEnterprisePlan()
        {
            var planType = Factory.BuildFrom(Plan.Enterprise);

            Assert.IsType<EnterprisePlan>(planType);
        }
    }
}
