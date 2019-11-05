using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace IntegrationTests
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer _server;
        public HttpClient Client { get; set; }

        public TestFixture()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            IWebHostBuilder builder =
                new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<TStartup>();

            _server = new TestServer(builder);
            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            _server.Dispose();
            Client.Dispose();
        }
    }
}
