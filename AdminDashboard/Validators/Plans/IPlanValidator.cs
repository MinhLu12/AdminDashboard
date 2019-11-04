using AdminDashboard.Main.Enumerations;

namespace AdminDashboard.Validators.Plans
{
    public interface IPlanValidator
    {
        bool IsPlanValid(Plan plan);
    }
}