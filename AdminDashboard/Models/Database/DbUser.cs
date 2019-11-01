namespace AdminDashboard.Models.Database
{
    public class DbUser
    {
        public string Id { get; set; }
        public virtual DbAccount Account { get; set; }
    }
}
