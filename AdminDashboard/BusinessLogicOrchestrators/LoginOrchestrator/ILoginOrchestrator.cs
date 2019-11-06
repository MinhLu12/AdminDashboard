namespace AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator
{
    public interface ILoginOrchestrator
    {
        string Authenticate(string username, string password);
    }
}
