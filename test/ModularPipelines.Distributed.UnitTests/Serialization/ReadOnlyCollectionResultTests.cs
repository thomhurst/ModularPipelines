using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ReadOnlyCollectionResultTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task Compiler_Collections_RoundTrip_Through_Declared_Contracts(int shape)
    {
        var first = new FilePath(Path.Combine(Directory.GetCurrentDirectory(), "first.csproj"));
        var second = new FilePath(Path.Combine(Directory.GetCurrentDirectory(), "second.csproj"));
        IReadOnlyList<FilePath> values = shape switch
        {
            0 => [],
            1 => [first],
            2 => [first, second],
            _ => [.. new[] { first, second }.Where(static _ => true)],
        };

        await AssertRoundTrip<IReadOnlyList<FilePath>>(values);
        await AssertRoundTrip<IReadOnlyCollection<FilePath>>(values);
        await AssertRoundTrip<IEnumerable<FilePath>>(values);
    }

    private static async Task AssertRoundTrip<T>(T values)
        where T : IEnumerable<FilePath>
    {
        var registry = new ModuleTypeRegistry();
        var moduleType = typeof(CollectionModule<T>);
        registry.Register(moduleType);
        var serializer = new ModuleResultSerializer(registry);
        var now = DateTimeOffset.UtcNow;
        var result = new ModuleResult<T>.Success(values)
        {
            Name = "CollectionModule",
            Duration = TimeSpan.Zero,
            StartTime = now,
            EndTime = now,
            Status = ModuleStatus.Succeeded,
        };

        var payload = serializer.Serialize(result, ModuleId.FromType(moduleType), workerIndex: 1);
        var restored = (ModuleResult<T>) serializer.Deserialize(payload)!;

        await Assert.That(restored.Value.Select(static file => file.Path)
            .SequenceEqual(values.Select(static file => file.Path))).IsTrue();
    }

    private abstract class CollectionModule<T> : Module<T>;
}
