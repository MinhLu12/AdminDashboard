using AdminDashboard.Main.Enumerations;

namespace AdminDashboard.Models.Plans
{
    public class StartupPlan : PlanType
    {
        public StartupPlan() : base(maxNumberOfUsersAllowed: 1, pricePerMonth: 100.00F) { }

        public override Plan GetPlanType() => Plan.Startup;
    }
}
