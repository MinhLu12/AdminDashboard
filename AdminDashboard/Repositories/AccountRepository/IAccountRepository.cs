using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Database;
using AdminDashboard.Models.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminDashboard.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        Task<Guid> Create(DbAccount account);

        Task<bool> UpgradePlan(Guid accountId, Plan plan);

        Task AddUserAndAccountRelationship(Guid accountId, Guid userId);

        Task<IEnumerable<User>> GetAccountUsers(Guid id);

        Task<Account> GetAccountBy(Guid id);
    }
}
