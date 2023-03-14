namespace EmployeeArrivalTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class PersonService : IPersonService
    {
        private readonly IDeletableEntityRepository<Person> personRepository;

        public PersonService(IDeletableEntityRepository<Person> personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<Person> AddAsync(Person person)
        {
            await this.personRepository.AddAsync(person);
            await this.personRepository.SaveChangesAsync();

            return person;
        }

        public async Task<IEnumerable<Person>> AddRangeAsync(IEnumerable<Person> people)
        {
            await this.personRepository.AddRangeAsync(people);
            await this.personRepository.SaveChangesAsync();

            return people;
        }

        public async Task<Person> GetByIdAsync(int id) => await this.personRepository.All().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Person>> GetByIdsAsync(IEnumerable<int> ids) => await this.personRepository.All().Where(e => ids.Contains(e.Id)).ToListAsync();

        public async Task<bool> CheckIfPersonExistsAsync(int id) => await this.personRepository.All().AnyAsync(p => p.Id == id);
    }
}
