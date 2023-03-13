namespace EmployeeArrivalTracker.Web
{
    using System;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data;
    using EmployeeArrivalTracker.Data.Common.Hubs;
    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Repositories;
    using EmployeeArrivalTracker.Services.Data;
    using EmployeeArrivalTracker.Services.Mapping;
    using EmployeeArrivalTracker.Web.Infrastructure;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EmployeesContext>(
                options => options
                                .UseLazyLoadingProxies()
                                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllers();
            services.AddHttpClient();
            services.AddMemoryCache();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSingleton(configuration);

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000")
                    .AllowCredentials();
                });
            });

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Services
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IArrivalService, ArrivalService>();
        }

        private static void Configure(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<Data.EmployeesContext>();
                dbContext.Database.Migrate();
            }

            AutoMapperConfig.RegisterMappings(
                typeof(EmployeeArrivalViewModel).GetTypeInfo().Assembly,
                typeof(EmployeeArrivalDetailsViewModel).GetTypeInfo().Assembly,
                typeof(AddEmployeeViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors("ClientPermission");

            app.UseMiddleware<SubscriptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<EmployeesHub>("/hubs/employees");
            });

            app.MapControllers();
        }
    }
}
