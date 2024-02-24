using Common.Persistence.EFCore.Tests.Helpers.Models;

namespace Common.Persistence.EFCore.Tests.Helpers.Repositories
{
    public class TestBaseRepository : BaseRepository<TestEntity, Guid, TestContext>
    {
        public TestBaseRepository(TestContext context)
            : base(context) { }
    }
}
