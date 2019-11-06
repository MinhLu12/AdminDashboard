using AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator;
using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator;
using AdminDashboard.Main.Configurations;
using AdminDashboard.Main.Databases;
using AdminDashboard.Repositories.AccountRepository;
using GravitationalTest.BusinessOrchestrators.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdminDashboard.Main
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureOrchestrators(services);
            ConfigureRepositories(services);
            ConfigureDatabase(services);
            ConfigureAuthorization(services);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // TODO: Remove
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            EnsureDatabaseSchemaIsMade(app);
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            IConfigurationSection section = BindIOptionsToAuthorizationConfiguration(services);

            var authorizationConfiguration = section.Get<AuthorizationConfiguration>();
            var key = Encoding.ASCII.GetBytes(authorizationConfiguration.Bearer);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        private IConfigurationSection BindIOptionsToAuthorizationConfiguration(IServiceCollection services)
        {
            var section = Configuration.GetSection("AuthorizationConfiguration");
            services.Configure<AuthorizationConfiguration>(section);

            return section;
        }

        private static void EnsureDatabaseSchemaIsMade(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AdminDashboardContext>();
            context.Database.EnsureCreated();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<AdminDashboardContext>(options => options.UseMySQL(Configuration["ConnectionStrings:DefaultConnection"]));
        }

        private void ConfigureOrchestrators(IServiceCollection services)
        {
            services.AddTransient<IAccountOrchestrator, AccountOrchestrator>();
            services.AddTransient<ILoginOrchestrator, LoginOrchestrator>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IAccountRepository, AccountRepository>();
        }
    }
}
