using AdminDashboard.Main.Enumerations;
using System;

namespace AdminDashboard.Validators.Plans
{
    public class PlanValidator : IPlanValidator
    {
        public bool IsPlanValid(Plan plan)
        {
            return Enum.IsDefined(typeof(Plan), plan);
        }
    }
}