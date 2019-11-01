using AdminDashboard.Main.Enumerations;
using System;
using System.Threading.Tasks;

namespace AdminDashboard.Validators.Plans
{
    public interface IPlanValidator
    {
        bool IsPlanValid(Plan plan);

        Task<bool> IsValidUpgradeTransition(Guid id, Plan proposedPlan);
    }
}
