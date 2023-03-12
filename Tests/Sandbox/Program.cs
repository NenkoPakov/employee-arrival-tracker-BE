namespace Sandbox
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using CommandLine;
    using EmployeeArrivalTracker.Common;
    using EmployeeArrivalTracker.Data;
    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Repositories;
    using EmployeeArrivalTracker.Data.Seeding;
    using EmployeeArrivalTracker.Services.Data;
    using EmployeeArrivalTracker.Services.Mapping;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Arrival;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            // Seed data on application startup
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<EmployeesContext>();
                dbContext.Database.Migrate();

                var seeder = new EmployeesContextSeeder();
                seeder.SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;

                return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                    opts => SandboxCode(opts, serviceProvider).GetAwaiter().GetResult(),
                    _ => 255);
            }
        }

        private static async Task<int> SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
        {
            var sw = Stopwatch.StartNew();

            var settingsService = serviceProvider.GetService<IEmployeeService>();
            Console.WriteLine($"Count of settings: {await settingsService.GetCountAsync()}");

            Console.WriteLine(sw.Elapsed);
            return await Task.FromResult(0);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(GlobalConstants.AppSettingsFileName, false, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging();
            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy(GlobalConstants.ClientPermissionsPolicyName, policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(GlobalConstants.FrontEndUrl)
                    .AllowCredentials();
                });
            });

            services.AddDbContext<EmployeesContext>(
                options => options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.ConnectionStringKey))
                    .UseLoggerFactory(new LoggerFactory()));

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IArrivalService, ArrivalService>();

            AutoMapperConfig.RegisterMappings(
                        typeof(EmployeeArrivalsViewModel).GetTypeInfo().Assembly,
                        typeof(EmployeeArrivalViewModel).GetTypeInfo().Assembly,
                        typeof(EmployeeArrivalDetailsViewModel).GetTypeInfo().Assembly,
                        typeof(AddEmployeeViewModel).GetTypeInfo().Assembly);
        }
    }
}
