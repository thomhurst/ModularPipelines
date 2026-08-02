using ModularPipelines.Caching;

namespace ModularPipelines.UnitTests.Caching;

public class MaximumLengthWriteStreamTests
{
    [Test]
    public async Task RejectedWriteDoesNotGrowUnderlyingStream()
    {
        await using var inner = new MemoryStream();
        var stream = new MaximumLengthWriteStream(inner, maximumLength: 4);
        await stream.WriteAsync(new byte[] { 1, 2, 3 });

        Assert.Throws<MaximumLengthExceededException>(() => stream.Write(new byte[] { 4, 5 }));

        using (Assert.Multiple())
        {
            await Assert.That(stream.Length).IsEqualTo(3);
            await Assert.That(inner.Length).IsEqualTo(3);
        }
    }
}
