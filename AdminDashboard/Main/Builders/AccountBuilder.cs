using AdminDashboard.Main.Factories;
using AdminDashboard.Models.Domain;
using AdminDashboard.Models.JsonRequests;

namespace AdminDashboard.Main.Builders
{
    public class AccountBuilder : IAccountBuilder
    {
        private readonly IPlanTypeFactory PlanTypeFactory;

        public AccountBuilder(IPlanTypeFactory planTypeFactory)
        {
            PlanTypeFactory = planTypeFactory;
        }

        public Account Build(CreateAccountRequest request)
        {
            var planType = PlanTypeFactory.BuildFrom(request.Plan);

            return new Account(planType);
        }
    }
}
