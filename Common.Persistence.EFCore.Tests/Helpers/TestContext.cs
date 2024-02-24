using Common.Persistence.EFCore.Tests.Helpers.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence.EFCore.Tests.Helpers
{
    public class TestContext : DbContext
    {
        public int CommitCounter { get; set; }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>()
                .HasKey(x=>x.Id);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CommitCounter++;
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
