using System;

namespace AdminDashboard.Models.EndUserRequests
{
    public class EndUserAccountUserRequest
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
    }
}
