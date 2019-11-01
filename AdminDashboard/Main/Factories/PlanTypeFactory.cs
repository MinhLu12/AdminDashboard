using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Plans;

namespace AdminDashboard.Main.Factories
{
    public class PlanTypeFactory : IPlanTypeFactory
    {
        public PlanType BuildFrom(Plan plan)
        {
            switch (plan)
            {
                case Plan.Startup:
                    return new StartupPlan();
                case Plan.Enterprise:
                    return new EnterprisePlan();
                default:
                    return null;
            }
        }
    }
}
