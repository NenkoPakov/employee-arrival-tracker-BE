namespace EmployeeArrivalTracker.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data;
    using Microsoft.Extensions.Configuration;

    public class EmployeesContextSeeder : ISeeder
    {
        private readonly string sourceFileDestination;
        private readonly int seederBatchSize;

        public EmployeesContextSeeder(string sourceFileDestination, int seederBatchSize)
        {
            this.sourceFileDestination = sourceFileDestination;
            this.seederBatchSize = seederBatchSize;
        }

        public async Task SeedAsync(EmployeesContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
                          {
                              new EmployeeSeeder(sourceFileDestination,seederBatchSize),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
