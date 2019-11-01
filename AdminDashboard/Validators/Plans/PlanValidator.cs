using AdminDashboard.Main.Enumerations;
using AdminDashboard.Repositories.AccountRepository;
using System;
using System.Threading.Tasks;

namespace AdminDashboard.Validators.Plans
{
    public class PlanValidator : IPlanValidator
    {
        private readonly IAccountRepository AccountRepository;

        public PlanValidator(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }

        public bool IsPlanValid(Plan plan)
        {
            return Enum.IsDefined(typeof(Plan), plan);
        }

        public async Task<bool> IsValidUpgradeTransition(Guid id, Plan proposedPlan)
        {
            var account = await AccountRepository.GetAccountBy(id);

            return IsProposedPlanHigherThanCurrentPlan(proposedPlan, account);
        }

        private static bool IsProposedPlanHigherThanCurrentPlan(Plan proposedPlan, Models.Domain.Account account)
        {
            return account.CurrentPlan.GetPlanType() < proposedPlan;
        }
    }
}
