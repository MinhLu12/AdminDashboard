using AdminDashboard.Exceptions;
using AdminDashboard.Main;
using AdminDashboard.Main.Enumerations;
using AdminDashboard.Models.EndUserRequests;
using AdminDashboard.Models.JsonRequests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.ControllerTests
{
    public class AccountControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        private readonly string ControllerUrl = "api/account";

        public AccountControllerTests(TestFixture<Startup> testFixture)
        {
            _client = testFixture.Client;
        }

        [Fact]
        public async Task CreateAccount__ReturnsAccount()
        {
            var request = GetCreateAccountRequest();
            Guid id = await CreateAccount(request);

            EnderUserAccountRequest account = await GetAccount(id);

            Assert.Equal(request.Plan, account.CurrentPlan);
            Assert.Empty(account.Users);
        }

        [Fact]
        public async Task UpgradePlan__ReturnsAccountWithNewPlan()
        {
            Guid id = await CreateAccount();
            var request = new UpgradePlanRequest() { Plan = Plan.Enterprise };

            await UpgradePlan(id, request);

            EnderUserAccountRequest account = await GetAccount(id);
            Assert.Equal(request.Plan, account.CurrentPlan);
        }

        [Fact]
        public async Task RegisterUserToAccount__ReturnsTrue()
        {
            Guid id = await CreateAccount();

            await RegisterUserToAccount(id);

            EnderUserAccountRequest account = await GetAccount(id);
            Assert.Single(account.Users);
        }

        [Fact]
        public async Task GetAccountUsers_WhenTwoExist_ReturnsBoth()
        {
            Guid id = await CreateAccount();
            int numberOfUsers = 2;
            await RegisterUserToAccount(id, times: numberOfUsers);

            IList<EndUserAccountUserRequest> accountUsers = await GetAccountUsers(id);

            Assert.Equal(numberOfUsers, accountUsers.Count);
        }

        [Fact]
        public async Task RegisterUserToAccount_WhenMaxNumberOfUsersReachedOnStartupPlan_ReturnsFalse()
        {
            Guid id = await CreateAccount();

            await RegisterUserToAccount(id, times: 100);

            await Assert.ThrowsAsync<UserLimitExceededException>(async () => await RegisterUserToAccount(id));
        }

        private async Task<Guid> CreateAccount()
        {
            var request = GetCreateAccountRequest();

            return await CreateAccount(request);
        }

        private async Task RegisterUserToAccount(Guid accountId)
        {
            Guid userId = Guid.NewGuid();

            await _client.PutAsync($"{ControllerUrl}/{accountId}/User/{userId}", null);
        }

        private async Task UpgradePlan(Guid id, UpgradePlanRequest request)
        {
            await _client.PutAsync($"{ControllerUrl}/{id}", GetHttpContentFromObj(request));
        }

        private async Task RegisterUserToAccount(Guid accountId, int times = 1)
        {
            for (int i = 0; i < times; i++) 
            {
                Guid userId = Guid.NewGuid();

                await _client.PutAsync($"{ControllerUrl}/{accountId}/User/{userId}", null);
            }
        }

        private async Task<EnderUserAccountRequest> GetAccount(Guid id)
        {
            HttpResponseMessage retrievedAccount = await _client.GetAsync($"{ControllerUrl}/{id}");

            return await GetObjFromHttpResponse<EnderUserAccountRequest>(retrievedAccount);
        }

        private async Task<IList<EndUserAccountUserRequest>> GetAccountUsers(Guid id)
        {
            HttpResponseMessage retrievedAccount = await _client.GetAsync($"{ControllerUrl}/{id}/Users");

            return await GetObjFromHttpResponse<IList<EndUserAccountUserRequest>>(retrievedAccount);
        }

        private async Task<Guid> CreateAccount(CreateAccountRequest request)
        {
            HttpResponseMessage createdAccount = await _client.PostAsync($"{ControllerUrl}", 
                GetHttpContentFromObj(request));

            return await GetObjFromHttpResponse<Guid>(createdAccount);
        }

        private static CreateAccountRequest GetCreateAccountRequest()
        {
            return new CreateAccountRequest() { Plan = Plan.Startup };
        }

        private static HttpContent GetHttpContentFromObj<T>(T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        private static async Task<T> GetObjFromHttpResponse<T>(HttpResponseMessage msg)
        {
            string content = await msg.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
