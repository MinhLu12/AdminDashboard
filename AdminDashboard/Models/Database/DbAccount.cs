using AdminDashboard.Main.Enumerations;
using System.Collections.Generic;

namespace AdminDashboard.Models.Database
{
    public class DbAccount
    {
        public string Id { get; set; }
        public Plan Plan { get; set; }
        public virtual ICollection<DbUser> Users { get; set; }
    }
}
