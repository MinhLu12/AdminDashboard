using AdminDashboard.Main.Enumerations;

namespace AdminDashboard.Models.Plans
{
    public abstract class PlanType
    {
        public int MaxNumberOfUsersAllowed;
        public float PricePerMonth;

        public PlanType(int maxNumberOfUsersAllowed, float pricePerMonth)
        {
            MaxNumberOfUsersAllowed = maxNumberOfUsersAllowed;
            PricePerMonth = pricePerMonth;
        }

        public abstract Plan GetPlanType();
    }
}
