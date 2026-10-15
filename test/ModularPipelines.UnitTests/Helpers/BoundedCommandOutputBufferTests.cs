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

        buffer.Append("01234".AsSpan());
        buffer.Append("56".AsSpan());
        buffer.Append("78".AsSpan());

        var result = buffer.ToString();
        using (Assert.Multiple())
        {
            await Assert.That(result).StartsWith("0123");
            await Assert.That(result).EndsWith("5678");
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

    [Test]
    public async Task Transforms_Truncation_Edges_After_Chunking_And_Tail_Wrap()
    {
        var buffer = new BoundedCommandOutputBuffer(8);
        foreach (var character in "0123456789abcdef")
        {
            buffer.Append(new ReadOnlySpan<char>(in character));
        }

        var fragments = new List<(string Value, bool Start, bool End)>();
        var result = buffer.ToString((value, start, end) =>
        {
            fragments.Add((value, start, end));
            return $"<{value}>";
        });

        await Assert.That(fragments).IsEquivalentTo(new[] { ("0123", false, true), ("cdef", true, false) });
        await Assert.That(result).IsEqualTo($"<0123>{Environment.NewLine}... [truncated 8 characters] ...{Environment.NewLine}<cdef>");
        await Assert.That(buffer.ToString()).StartsWith("0123");
        await Assert.That(buffer.ToString()).EndsWith("cdef");
    }
}
