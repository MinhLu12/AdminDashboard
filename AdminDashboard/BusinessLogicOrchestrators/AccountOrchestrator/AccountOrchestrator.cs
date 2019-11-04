using AdminDashboard.Exceptions;
using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Domain;
using AdminDashboard.Models.EndUserRequests;
using AdminDashboard.Models.JsonRequests;
using AdminDashboard.Models.Plans;
using AdminDashboard.Repositories.AccountRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator
{
    public class AccountOrchestrator : IAccountOrchestrator
    {
        private readonly IAccountRepository Repository;

        public AccountOrchestrator(IAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task RegisterUserToAccount(Guid accountId, Guid userId)
        {
            var account = await Repository.GetAccountBy(accountId);

            if (DoesntExist(account))
                throw new AccountNotFoundException();

            if (HasUser(userId, account))
                throw new DuplicateUserOnAccountException();

            if (account.Users.Count == account.CurrentPlan.MaxNumberOfUsersAllowed)
                throw new UserLimitExceededException();

            await Repository.AddUser(accountId, userId);
        }

        public async Task UpgradePlan(Guid accountId, UpgradePlanRequest request)
        {
            var account = await Repository.GetAccountBy(accountId);

            if (DoesntExist(account))
                throw new AccountNotFoundException();
            if (!IsPlanValid(request.Plan))
                throw new InvalidPlanException();
            if (!IsProposedPlanHigherThanCurrentPlan(request.Plan, account))
                throw new InvalidUpgradePlanException();

            await Repository.UpgradePlan(accountId, request.Plan);
        }

        public async Task<EnderUserAccountRequest> GetAccountBy(Guid id)
        {
            var account = await Repository.GetAccountBy(id);

            if (DoesntExist(account))
                throw new AccountNotFoundException();

            return account.ToEndUser();
        }

        public async Task<IEnumerable<EndUserAccountUserRequest>> GetAccountUsers(Guid accountId)
        {
            var account = await Repository.GetAccountBy(accountId);

            if (DoesntExist(account))
                throw new AccountNotFoundException();

            IEnumerable<User> accountUsers = await Repository.GetAccountUsers(accountId);

            return ConstructEndUserRequestsFrom(accountUsers);
        }

        public async Task<Guid> CreateAccount(CreateAccountRequest request)
        {
            if (!IsPlanValid(request.Plan))
                throw new InvalidPlanException();

            var plan = GetPlanTypeFrom(request);

            var account = new Account(plan);

            return await Repository.Create(account.ToDatabase());
        }

        private bool IsPlanValid(Plan plan)
        {
            return Enum.IsDefined(typeof(Plan), plan);
        }

        private static bool HasUser(Guid userId, Account account)
        {
            return account.Users.Exists(u => u == userId);
        }

        private static bool IsProposedPlanHigherThanCurrentPlan(Plan proposedPlan, Account account)
        {
            return account.CurrentPlan.GetPlanType() < proposedPlan;
        }

        private static bool DoesntExist(Account domain)
        {
            return domain == null;
        }

        private static PlanType GetPlanTypeFrom(CreateAccountRequest request)
        {
            PlanType plan = null;
            switch (request.Plan)
            {
                case Plan.Startup:
                    plan = new StartupPlan();
                    break;
                case Plan.Enterprise:
                    plan = new EnterprisePlan();
                    break;
            }

            return plan;
        }

        private static IEnumerable<EndUserAccountUserRequest> ConstructEndUserRequestsFrom(IEnumerable<User> accountUsers)
        {
            return accountUsers.Select(a => ConstructEndUserRequestFrom(a));
        }

        private static EndUserAccountUserRequest ConstructEndUserRequestFrom(User a)
        {
            return new EndUserAccountUserRequest() { AccountId = a.AccountId, UserId = a.Id };
        }
    }
}
