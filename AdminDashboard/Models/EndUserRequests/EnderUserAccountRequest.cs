using AdminDashboard.Main.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminDashboard.Models.EndUserRequests
{
    public class EnderUserAccountRequest
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> Users { get; set; }
        public Plan CurrentPlan { get; set; }
    }
}
