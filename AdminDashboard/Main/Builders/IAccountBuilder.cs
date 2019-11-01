using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.Domain;
using AdminDashboard.Models.JsonRequests;

namespace AdminDashboard.Main.Builders
{
    public interface IAccountBuilder
    {
        Account Build(CreateAccountRequest request);
    }
}
