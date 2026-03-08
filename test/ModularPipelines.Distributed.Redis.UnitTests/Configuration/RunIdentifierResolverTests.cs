using ModularPipelines.Distributed.Redis.Configuration;

namespace ModularPipelines.Distributed.Redis.UnitTests.Configuration;

public class RunIdentifierResolverTests
{
    [Test]
    public async Task Resolve_WithExplicitValue_ReturnsExplicitValue()
    {
        var result = RunIdentifierResolver.Resolve("my-explicit-id");

        await Assert.That(result).IsEqualTo("my-explicit-id");
    }

    [Test]
    public async Task Resolve_WithNullExplicit_DoesNotReturnNull()
    {
        var result = RunIdentifierResolver.Resolve(null);

        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsNotEqualTo(string.Empty);
    }

    [Test]
    public async Task Resolve_WithEmptyExplicit_DoesNotReturnEmpty()
    {
        var result = RunIdentifierResolver.Resolve(string.Empty);

        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsNotEqualTo(string.Empty);
    }

    [Test]
    public async Task Resolve_WithWhitespaceExplicit_DoesNotReturnWhitespace()
    {
        var result = RunIdentifierResolver.Resolve("   ");

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Trim()).IsNotEqualTo(string.Empty);
    }

    [Test]
    public async Task Resolve_ReturnsConsistentValueForSameExplicit()
    {
        var result1 = RunIdentifierResolver.Resolve("test-run-123");
        var result2 = RunIdentifierResolver.Resolve("test-run-123");

        await Assert.That(result1).IsEqualTo(result2);
    }

    [Test]
    public async Task Resolve_WithoutExplicit_ReturnsFallback()
    {
        var result = RunIdentifierResolver.Resolve(null);

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsGreaterThan(0);
    }
}
