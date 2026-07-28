using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Helpers;

public class BoundedCommandOutputBufferTests
{
    [Test]
    public async Task Retains_Output_Within_Limit()
    {
        var buffer = new BoundedCommandOutputBuffer(10);

        buffer.Append("abc\n123".AsSpan());

        await Assert.That(buffer.ToString()).IsEqualTo("abc\n123");
    }

    [Test]
    public async Task Retains_Head_And_Tail_When_Truncated()
    {
        var buffer = new BoundedCommandOutputBuffer(10);

        buffer.Append("0123".AsSpan());
        buffer.Append("456789".AsSpan());
        buffer.Append("ABCDEF".AsSpan());

        var result = buffer.ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result).StartsWith("01234");
            await Assert.That(result).Contains("truncated 6 characters");
            await Assert.That(result).EndsWith("BCDEF");
        }
    }

    [Test]
    public async Task Retains_Latest_Tail_Across_Buffer_Wrap()
    {
        var buffer = new BoundedCommandOutputBuffer(8);

        buffer.Append("012345".AsSpan());
        buffer.Append("6789".AsSpan());

        var result = buffer.ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result).StartsWith("0123");
            await Assert.That(result).EndsWith("6789");
        }
    }

    [Test]
    public async Task NonPositive_Limit_Retains_Unlimited_Output()
    {
        var buffer = new BoundedCommandOutputBuffer(0);
        var output = new string('x', 100);

        buffer.Append(output.AsSpan());

        await Assert.That(buffer.ToString()).IsEqualTo(output);
    }
}
