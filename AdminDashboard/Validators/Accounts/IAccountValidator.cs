using AdminDashboard.Main.Enumerations;
using System;
using System.Threading.Tasks;

namespace AdminDashboard.Validators.Accounts
{
    public interface IAccountValidator
    {
        Task<bool> DoesAccountExist(Guid id);

        Task<bool> DoesAccountAlreadyHaveUser(Guid accountId, Guid userId);

        Task<bool> HasAccountReachedMaximumNumberOfUsers(Guid accountId);
    }
}
