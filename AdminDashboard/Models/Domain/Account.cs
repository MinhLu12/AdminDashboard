using AdminDashboard.Models.Database;
using AdminDashboard.Models.EndUserRequests;
using AdminDashboard.Models.Plans;
using System;
using System.Collections.Generic;

namespace AdminDashboard.Models.Domain
{
    public class Account
    {
        public Guid Id { get; }
        public List<Guid> Users { get; }
        public PlanType CurrentPlan { get; }

        public Account(PlanType plan)
        {
            Id = Guid.NewGuid();
            Users = new List<Guid>();
            CurrentPlan = plan;
        }

        public Account(Guid id, List<Guid> currentUsers, PlanType plan)
        {
            Id = id;
            Users = currentUsers;
            CurrentPlan = plan;
        }

        public void RegisterUser(Guid id)
        {
            Users.Add(id);
        }

        public EnderUserAccountRequest ToEndUser()
        {
            return new EnderUserAccountRequest()
            {
                Id = Id,
                Users = Users,
                CurrentPlan = CurrentPlan.GetPlanType(),
                MaximumNumberOfUsersAllowed = CurrentPlan.MaxNumberOfUsersAllowed,
                PricePerMonth = CurrentPlan.PricePerMonth
            };
        }

        public DbAccount ToDatabase()
        {
            return new DbAccount()
            {
                Id = Id.ToString(),
                Plan = CurrentPlan.GetPlanType()
            };
        }
    }
}
