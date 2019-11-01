using System;

namespace AdminDashboard.Models.Domain
{
    public class User
    {
        public Guid Id { get; }
        public Guid AccountId { get; }

        public User(Guid id, Guid accountId)
        {
            Id = id;
            AccountId = accountId;
        }
    }
}
