﻿using AdminDashboard.Models.EndUserRequests;
using AdminDashboard.Models.JsonRequests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator
{
    public interface IAccountOrchestrator
    {
        Task RegisterUserToAccount(Guid accountId, Guid userId);

        Task UpgradePlan(Guid accountId, UpgradePlanRequest request);

        Task<Guid> CreateAccount(CreateAccountRequest request);

        Task<EnderUserAccountRequest> GetAccountBy(Guid id);

        Task<IEnumerable<EndUserAccountUserRequest>> GetAccountUsers(Guid accountId);
    }
}
