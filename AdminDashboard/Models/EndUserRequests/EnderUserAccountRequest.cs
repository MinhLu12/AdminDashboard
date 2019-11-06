using AdminDashboard.Main.Enumerations;
using System;
using System.Collections.Generic;

namespace AdminDashboard.Models.EndUserRequests
{
    public class EnderUserAccountRequest
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> Users { get; set; }
        public Plan CurrentPlan { get; set; }
        public int MaximumNumberOfUsersAllowed { get; set; }
        public float PricePerMonth { get; set; }
    }
}
