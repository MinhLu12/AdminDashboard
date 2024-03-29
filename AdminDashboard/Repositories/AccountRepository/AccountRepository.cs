﻿using AdminDashboard.Main.Databases;
using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Database;
using AdminDashboard.Models.Domain;
using AdminDashboard.Models.Plans;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminDashboard.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AdminDashboardContext Context;

        public AccountRepository(AdminDashboardContext context)
        {
            Context = context;
        }

        public async Task<Guid> Create(DbAccount account)
        {
            await Context.Account.AddAsync(account);

            await Context.SaveChangesAsync();

            return Guid.Parse(account.Id);
        }

        public async Task UpgradePlan(Guid accountId, Plan plan)
        {
            DbAccount dbAccount = await GetAccount(accountId);
            dbAccount.Plan = plan;

            await Context.SaveChangesAsync();
        }


        public async Task AddUser(Guid accountId, Guid userId)
        {
            Context.User.Add(new DbUser
            {
                Id = userId.ToString(),
                Account = await GetAccount(accountId)
            });

            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAccountUsers(Guid id)
        {
            var accountUsers = await GetUsersOnAccount(id);

            return accountUsers.Select(db => ToDomain(db, id));
        }

        public async Task<Account> GetAccountBy(Guid id)
        {
            DbAccount dbAccount = await GetAccount(id);
            if (AccountDoesNotExist(dbAccount))
                return null;

            List<DbUser> dbUsers = await GetUsersOnAccount(id);

            PlanType plan = GetPlanFrom(dbAccount);

            return BuildAndReturnAccount(id, plan, dbUsers);
        }

        private static PlanType GetPlanFrom(DbAccount dbAccount)
        {
            PlanType plan;
            if (dbAccount.Plan == Plan.Startup)
            {
                plan = new StartupPlan();
            }
            else
            {
                plan = new EnterprisePlan();
            }

            return plan;
        }

        private async Task<List<DbUser>> GetUsersOnAccount(Guid id)
        {
            return await Context.User.Where(u => u.Account.Id == id.ToString()).ToListAsync();
        }

        private async Task<DbAccount> GetAccount(Guid id)
        {
            return await Context.Account.FindAsync(id.ToString());
        }

        private static bool AccountDoesNotExist(DbAccount dbAccount)
        {
            return dbAccount == null;
        }

        private static User ToDomain(DbUser u, Guid accountId)
        {
            return new User(Guid.Parse(u.Id), accountId);
        }

        private static Account BuildAndReturnAccount(Guid id, PlanType plan, List<DbUser> dbUsers)
        {
            var listsOfUsers = DoesAccountHaveUsers(dbUsers) ? 
                dbUsers.Select(u => Guid.Parse(u.Id)).ToList()
                : new List<Guid>();

            return new Account(id, listsOfUsers, plan);
        }

        private static bool DoesAccountHaveUsers(List<DbUser> dbUsers)
        {
            return dbUsers.Count() != 0;
        }
    }
}
