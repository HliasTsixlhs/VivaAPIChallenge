using Common.Persistence.EFCore.Tests.Helpers.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence.EFCore.Tests.Helpers
{
	public class InitTestData: IDisposable
	{
		private DbContext _dbContext;
		private DbSet<TestEntity> _dbSet;

		public InitTestData(DbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<TestEntity>();
		}

		public void Dispose()
		{
			_dbSet = null;
			_dbContext = null;
		}

		public IEnumerable<TestEntity> PopulateWithEntities(int numberOfEntities)
		{
			var entitiesAdded = new List<TestEntity>();

			for(var i=0; i<numberOfEntities;i++)
			{
				var testEntity = new TestEntity
				{
					Id = Guid.NewGuid(),
					Name = $"EntityNumber{i}"
				};
				entitiesAdded.Add(testEntity);
				_dbSet.Add(testEntity);
			}
			_dbContext.SaveChanges();
			return entitiesAdded;
		}

        public static IEnumerable<TestEntity> GenerateTestEntity(int numberOfEntities)
		{
			var entitiesCreated = new List<TestEntity>();
			for (var i = 0; i < numberOfEntities; i++)
			{
				var id = Guid.NewGuid();
				var entity =  new TestEntity
				{
					Id = id,
					Name = $"EntityWithId:{id}"
				};
				entitiesCreated.Add(entity);
			}
			return entitiesCreated;
		}

	}
}
