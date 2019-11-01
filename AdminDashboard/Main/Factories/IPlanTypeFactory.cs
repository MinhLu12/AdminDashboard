using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Plans;

namespace AdminDashboard.Main.Factories
{
    public interface IPlanTypeFactory
    {
        PlanType BuildFrom(Plan plan);
    }
}
