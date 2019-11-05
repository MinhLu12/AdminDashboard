using AdminDashboard.BusinessLogicOrchestrators.AccountOrchestrator;
using AdminDashboard.Main.Databases;
using AdminDashboard.Repositories.AccountRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

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

            services.AddControllers();

            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UseSwagger(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            EnsureDatabaseSchemaIsMade(app);
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

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin Dashboard API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private static void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin Dashboard API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        private void ConfigureOrchestrators(IServiceCollection services)
        {
            services.AddTransient<IAccountOrchestrator, AccountOrchestrator>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IAccountRepository, AccountRepository>();
        }
    }
}
