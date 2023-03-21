namespace EmployeeArrivalTracker.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EmployeeArrivalTracker.Data.Common.Repositories;
    using EmployeeArrivalTracker.Data.Models;

    using Moq;

    using Xunit;

    public class PersonServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Person>> mockPersonRepository;
        private readonly IPersonService personService;

        public PersonServiceTests()
        {
            this.mockPersonRepository = new Mock<IDeletableEntityRepository<Person>>();
            this.personService = new PersonService(this.mockPersonRepository.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldCallAddAsyncAndSaveChangesAsync()
        {
            // Arrange
            var person = new Person();

            // Act
            await this.personService.AddAsync(person);

            // Assert
            this.mockPersonRepository.Verify(x => x.AddAsync(person), Times.Once);
            this.mockPersonRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddRangeAsync_ShouldCallAddRangeAsyncAndSaveChangesAsync()
        {
            // Arrange
            var people = new List<Person> { new Person(), new Person() };

            // Act
            await this.personService.AddRangeAsync(people);

            // Assert
            this.mockPersonRepository.Verify(x => x.AddRangeAsync(people), Times.Once);
            this.mockPersonRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
