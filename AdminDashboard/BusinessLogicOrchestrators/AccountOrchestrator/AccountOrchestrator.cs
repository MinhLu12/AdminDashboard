using AdminDashboard.Main.Builders;
using AdminDashboard.Models.Domain;
using AdminDashboard.Models.EndUserRequests;
using AdminDashboard.Models.JsonRequests;
using AdminDashboard.Repositories.AccountRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator
{
    public class AccountOrchestrator : IAccountOrchestrator
    {
        private readonly IAccountBuilder Builder;
        private readonly IAccountRepository Repository;

        public AccountOrchestrator(IAccountBuilder builder,
            IAccountRepository repository)
        {
            Builder = builder;
            Repository = repository;
        }

        public async Task<EnderUserAccountRequest> GetAccountBy(Guid id)
        {
            Account domain = await Repository.GetAccountBy(id);

            return domain.ToEndUser();
        }

        public async Task<IEnumerable<EndUserAccountUserRequest>> GetAccountUsers(Guid accountId)
        {
            IEnumerable<User> accountUsers = await Repository.GetAccountUsers(accountId);

            return ConstructEndUserRequestsFrom(accountUsers);
        }

        public async Task<Guid> CreateAccount(CreateAccountRequest request)
        {
            Account domain = Builder.Build(request);

            return await Repository.Create(domain.ToDatabase());
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
