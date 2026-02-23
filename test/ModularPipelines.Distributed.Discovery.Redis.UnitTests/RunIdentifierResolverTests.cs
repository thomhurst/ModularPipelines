using ModularPipelines.Distributed.Discovery.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RunIdentifierResolverTests
{
    [Test]
    public async Task Returns_Configured_Value_When_Provided()
    {
        var result = RunIdentifierResolver.Resolve("my-run-id");
        await Assert.That(result).IsEqualTo("my-run-id");
    }

    [Test]
    public async Task Returns_Hash_When_Null()
    {
        var result = RunIdentifierResolver.Resolve(null);

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(12);
    }

    [Test]
    public async Task Returns_Hash_When_Empty()
    {
        var result = RunIdentifierResolver.Resolve("");

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(12);
    }

    [Test]
    public async Task Returns_Hash_When_Whitespace()
    {
        var result = RunIdentifierResolver.Resolve("   ");

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsEqualTo(12);
    }

    [Test]
    public async Task Same_Directory_Produces_Same_Hash()
    {
        var result1 = RunIdentifierResolver.Resolve(null);
        var result2 = RunIdentifierResolver.Resolve(null);

        await Assert.That(result1).IsEqualTo(result2);
    }
}
