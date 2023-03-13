namespace EmployeeArrivalTracker.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Common;
    using EmployeeArrivalTracker.Data;
    using EmployeeArrivalTracker.Services.Data;
    using EmployeeArrivalTracker.Web.ViewModels.Employee.Add;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    internal class EmployeeSeeder : ISeeder
    {
        private readonly string sourceFileDestination;
        private readonly int seederBatchSize;

        public EmployeeSeeder(string sourceFileDestination, int seederBatchSize)
        {
            this.sourceFileDestination = sourceFileDestination;
            this.seederBatchSize = seederBatchSize;
        }

        public async Task SeedAsync(EmployeesContext dbContext, IServiceProvider serviceProvider)
        {
            var employeeService = serviceProvider.GetRequiredService<IEmployeeService>();

            using (StreamReader file = File.OpenText(this.sourceFileDestination))
            {
                JsonTextReader reader = new JsonTextReader(file);

                ICollection<AddEmployeeViewModel> employees = new List<AddEmployeeViewModel>();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var deserializedEmployee = JsonSerializer.Create().Deserialize<AddEmployeeViewModel>(reader);
                        employees.Add(deserializedEmployee);

                        if (employees.Count == this.seederBatchSize)
                        {
                            await SeedEmployeesAsync(employeeService, employees);
                            employees.Clear();
                        }
                    }
                }

                if (employees.Count > 0)
                {
                    await SeedEmployeesAsync(employeeService, employees);
                }
            }
        }

        private static async Task SeedEmployeesAsync(IEmployeeService employeeService, IEnumerable<AddEmployeeViewModel> employees)
        {
            await employeeService.AddRangeAsync(employees);
        }
    }
}
