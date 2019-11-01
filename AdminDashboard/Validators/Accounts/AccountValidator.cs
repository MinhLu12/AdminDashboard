using AdminDashboard.Repositories.AccountRepository;
using System;
using System.Threading.Tasks;

namespace AdminDashboard.Validators.Accounts
{
    public class AccountValidator : IAccountValidator
    {
        private readonly IAccountRepository Repository;

        public AccountValidator(IAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<bool> DoesAccountExist(Guid id)
        {
            return await Repository.GetAccountBy(id) == null ? false : true;
        }

        public async Task<bool> DoesAccountAlreadyHaveUser(Guid accountId, Guid userId)
        {
            var account = await Repository.GetAccountBy(accountId);

            return account.Users.Exists(u => u == userId);
        }

        public async Task<bool> HasAccountReachedMaximumNumberOfUsers(Guid accountId)
        {
            var account = await Repository.GetAccountBy(accountId);

            return account.Users.Count == account.CurrentPlan.MaxNumberOfUsersAllowed;
        }
    }
}
