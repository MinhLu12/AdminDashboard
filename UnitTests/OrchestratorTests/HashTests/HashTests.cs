using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator;
using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator.PasswordHasher;
using Xunit;

namespace UnitTests.OrchestratorTests.HashTests
{
    public class HashTests
    {
        private readonly string Message = "Password";

        [Fact]
        public void Hash__IsMatch()
        {
            var salt = Salt.Create();
            var hash = Hash.Create(Message, salt);

            var match = Hash.Validate(Message, salt, hash);

            Assert.True(match);
        }

        [Fact]
        public void Hash_WhenIncorrectCredentials_IsNotMatch()
        {
            var salt = Salt.Create();
            var hash = "DifferentPassword";

            var match = Hash.Validate(Message, salt, hash);

            Assert.False(match);
        }
    }
}
