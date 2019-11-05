using AdminDashboard.Main.Enumerations;

namespace AdminDashboard.Models.Plans
{
    public class EnterprisePlan : PlanType
    {
        public EnterprisePlan() : base(maxNumberOfUsersAllowed: 1000, pricePerMonth: 1000.00F) { }

        public override Plan GetPlanType() => Plan.Enterprise;
    }
}
