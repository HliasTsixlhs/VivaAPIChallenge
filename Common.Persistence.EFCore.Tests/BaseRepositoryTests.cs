using Common.Persistence.EFCore.Tests.Helpers;
using Common.Persistence.EFCore.Tests.Helpers.Models;
using Common.Persistence.EFCore.Tests.Helpers.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence.EFCore.Tests
{
    public class BaseRepositoryTests : IDisposable
    {
        private TestBaseRepository _testRepository;
        private readonly TestContext _dbContext;
        private readonly InitTestData _dataInit;
        private DbSet<TestEntity> _dbSet;

        public BaseRepositoryTests()
        {
            /*Added guid to database name because otherwise the tests
            that normally have their own context each since they aren't part of a fixture
            may accidentally share the database if it has the same name*/
            var options = new DbContextOptionsBuilder<TestContext>()
                .UseInMemoryDatabase(databaseName: $"InMemoryDatabase{Guid.NewGuid().ToString()}")
                .Options;
            _dbContext = new TestContext(options);
            _dbContext.Database.EnsureCreated();
            _dbSet = _dbContext.Set<TestEntity>();
            _testRepository = new TestBaseRepository(_dbContext);
            _dataInit = new InitTestData(_dbContext);
        }

        public void Dispose()
        {
            _testRepository = null;
            _dbSet = null;
            _dataInit.Dispose();
            _dbContext.Dispose();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        public async void GetAllAsync_ShouldReturnAll(int numberOfEntities)
        {
            var entitiesAdded = _dataInit.PopulateWithEntities(numberOfEntities);

            var result = await _testRepository.GetAllAsync();

            result.Should()
                .HaveCount(numberOfEntities)
                .And.Equal(entitiesAdded);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmpty_WhenEmpty()
        {
            var result = await _testRepository.GetAllAsync();

            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        public async void GetAsync_ShouldReturnValue_WhenValueIsPresent(int numberOfEntities, int entityIndex)
        {
            var entitiesAdded = _dataInit.PopulateWithEntities(numberOfEntities);
            var entity = ((List<TestEntity>) entitiesAdded)[entityIndex];

            var result = await _testRepository.GetAsync(entity.Id);

            result.Should()
                .NotBeNull()
                .And.BeEquivalentTo(entity);
        }

        [Fact]
        public async void GetAsync_ShouldBeNull_WhenValueIsNotPresent()
        {
            _dataInit.PopulateWithEntities(3);

            var result = await _testRepository.GetAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public void AsQueryable_ShouldNotBeNull()
        {
            var result = _testRepository.AsQueryable();

            result.Should().NotBeNull();
        }

        [Fact]
        public async void SearchAsync_ShouldReturnEntity_WhenEntityExists()
        {
            var entitiesAdded = _dataInit.PopulateWithEntities(3);

            var result = await _testRepository.SearchAsync(x => x.Name == entitiesAdded.ToList().First().Name);

            result.Should().BeEquivalentTo(new[] {entitiesAdded.ToList().FirstOrDefault()});
        }

        [Fact]
        public async void SearchAsync_ShouldReturnMany_WhenEntitiesExists()
        {
            _dataInit.PopulateWithEntities(3);

            var result = await _testRepository.SearchAsync(x => x.Name != string.Empty);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async void SearchAsync_ShouldReturnEmpty_WhenEntityDoesNotExists()
        {
            var result = await _testRepository.SearchAsync(x => x.Name == "Doesn't exist");

            result.Should().BeEmpty();
        }

        [Fact]
        public void Create_ShouldAddEntity_WhenItDoesNotExistAlready()
        {
            var entity = InitTestData.GenerateTestEntity(1).FirstOrDefault();
            _testRepository.Create(entity);

            if (entity == null) return;
            var result = _dbSet.Find(entity.Id);

            result.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public void Create_ShouldThrowException_WhenItExistsAlready()
        {
            var entityAdded = _dataInit.PopulateWithEntities(1);

            _testRepository.Create(entityAdded);
            //Func instead of Action for asynchronous methods
            var f = async () => { await _testRepository.CommitAsync(); };
            f.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void Create_ShouldNotSaveChangesOnItsOwn_WhenAddingOneEntity()
        {
            var entity = InitTestData.GenerateTestEntity(1).FirstOrDefault();
            _testRepository.Create(entity);

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void Create_ShouldAddEntities_WhenEntitiesDoNotExistAlready()
        {
            var entities = InitTestData.GenerateTestEntity(3);
            var testEntities = entities as TestEntity[] ?? entities.ToArray();
            await _testRepository.CreateAsync(testEntities);
            await _testRepository.CommitAsync();

            var result = _dbSet.ToList();

            result.Should().BeEquivalentTo(testEntities);
        }

        [Fact]
        public void Create_ShouldThrowException_WhenEntitiesExistAlready()
        {
            var entities = _dataInit.PopulateWithEntities(5);

            _testRepository.Create(entities.ToList().GetRange(0, 2));
            //Func instead of Action for asynchronous methods
            var f = async () => { await _testRepository.CommitAsync(); };
            f.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void Create_ForMultipleEntities_ShouldNotSaveChangesOnItsOwn()
        {
            var entities = _dataInit.PopulateWithEntities(5);

            _testRepository.Create(entities);

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void CreateAsync_ShouldAddEntity_WhenItDoesNotExistAlready()
        {
            var entity = InitTestData.GenerateTestEntity(1).FirstOrDefault();
            await _testRepository.CreateAsync(entity);

            if (entity == null) return;
            var result = await _dbSet.FindAsync(entity.Id);

            result.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async void CreateAsync_ShouldThrowException_WhenItExistsAlready()
        {
            var entityAdded = _dataInit.PopulateWithEntities(1);

            await _testRepository.CreateAsync(entityAdded);
            //Func instead of Action for asynchronous methods
            var f = async () => { await _testRepository.CommitAsync(); };
            await f.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateAsync_ShouldNotSaveChangesOnItsOwn_WhenAddingOneEntity()
        {
            var entity = InitTestData.GenerateTestEntity(1).FirstOrDefault();
            await _testRepository.CreateAsync(entity);

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void CreateAsync_ShouldAddEntities_WhenEntitiesDoNotExistAlready()
        {
            var entities = InitTestData.GenerateTestEntity(3);
            var testEntities = entities as TestEntity[] ?? entities.ToArray();
            await _testRepository.CreateAsync(testEntities);
            await _testRepository.CommitAsync();

            var result = _dbSet.ToList();

            result.Should().BeEquivalentTo(testEntities);
        }

        [Fact]
        public async void CreateAsync_ShouldThrowException_WhenEntitiesExistAlready()
        {
            var entities = _dataInit.PopulateWithEntities(5);

            await _testRepository.CreateAsync(entities.ToList().GetRange(0, 2));
            //Func instead of Action for asynchronous methods
            var f = async () => { await _testRepository.CommitAsync(); };
            await f.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void CreateAsync_ForMultipleEntities_ShouldNotSaveChangesOnItsOwn()
        {
            var entities = InitTestData.GenerateTestEntity(3);
            await _testRepository.CreateAsync(entities);

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void RemoveAsync_ShouldRemoveEntity_WhenEntityExists()
        {
            var entities = _dataInit.PopulateWithEntities(5);
            var entityRemoved = await _testRepository.RemoveAsync(entities.FirstOrDefault()!.Id);
            await _testRepository.CommitAsync();
            var result = _dbSet.ToList();

            result.Should().NotContain(entityRemoved); //Seems NotContain(object) checks if equal not if same object
        }

        [Fact]
        public async void RemoveAsync_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            _dataInit.PopulateWithEntities(5);

            var result = await _testRepository.RemoveAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        [Fact]
        public async void RemoveAsync_ShouldNotSaveChangesOnItsOwn()
        {
            var entities = _dataInit.PopulateWithEntities(5);
            await _testRepository.RemoveAsync(entities.FirstOrDefault()!.Id);

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void UpdateAsync_ShouldUpdateEntity_WhenEntityExists()
        {
            var entities = _dataInit.PopulateWithEntities(5);
            var id = entities.First().Id;
            const string newName = "NewName";
            await _testRepository.UpdateAsync(id, new TestEntity {Id = id, Name = newName});
            await _testRepository.CommitAsync();

            var result = await _dbSet.FindAsync(id);

            result?.Name.Should().BeEquivalentTo(newName);
        }

        [Fact]
        public async void UpdateAsync_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            _dataInit.PopulateWithEntities(5);
            var id = Guid.NewGuid();
            var result = await _testRepository.UpdateAsync(id, new TestEntity {Id = id, Name = "NewName"});

            result.Should().BeNull();
        }

        [Fact]
        public async void UpdateAsync_ShouldNotSaveChangesOnItsOwn()
        {
            var entities = _dataInit.PopulateWithEntities(5);
            var id = entities.First().Id;
            var newName = "NewName";
            await _testRepository.UpdateAsync(id, new TestEntity {Id = id, Name = newName});

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void UpdateAsync_Overload_ShouldNotSaveChangesOnItsOwn()
        {
            _dataInit.PopulateWithEntities(5);
            var id = Guid.NewGuid();
            var newEntity = new TestEntity
            {
                Id = id,
                Name = "NewName"
            };
            await _testRepository.UpdateAsync(id, newEntity);

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void UpdateAsync_ShouldUpdateEntities_WhenSomeOfThemExist()
        {
            var entities = _dataInit.PopulateWithEntities(5);
            var entitiesToUpdate = entities.ToList().GetRange(0, 2);
            entitiesToUpdate[0].Name = "NewName1";
            entitiesToUpdate[1].Name = "NewName2";
            var newEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "NewName3"
            };
            entitiesToUpdate.Add(newEntity);
            var entitiesDictionary = entitiesToUpdate.ToDictionary(entity => entity.Id);

            await _testRepository.UpdateAsync(entitiesDictionary);

            var result = _dbSet.ToList();

            //Seems NotContain(object) checks if equal not if same object
            result.Should().Contain(entitiesToUpdate[0])
                .And.Contain(entitiesToUpdate[1])
                .And.NotContain(entitiesToUpdate[2]);
        }

        [Fact]
        public async void UpdateAsync_ForMultipleEntities_ShouldNotSaveChangesOnItsOwn()
        {
            var entities = _dataInit.PopulateWithEntities(2).ToList();
            entities[0].Name = "NewName1";
            entities[1].Name = "NewName2";
            var newEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "NewName3"
            };
            entities.Add(newEntity);
            var entitiesDictionary = entities.ToDictionary(entity => entity.Id);

            await _testRepository.UpdateAsync(entitiesDictionary);

            _dbContext.CommitCounter.Should().Be(0);
        }

        [Fact]
        public async void CommitAsync_ShouldCallSaveChanges()
        {
            await _testRepository.CommitAsync();

            _dbContext.CommitCounter.Should().Be(1);
        }
    }
}