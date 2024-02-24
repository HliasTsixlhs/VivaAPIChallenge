namespace Common.Persistence.EFCore.Tests.Helpers.Models
{
    public class TestEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreationTime { get; set; }
    }
}
