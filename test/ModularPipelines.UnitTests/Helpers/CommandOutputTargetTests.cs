using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandOutputTargetTests
{
    [Test]
    [Arguments("a\nb\n", new[] { "a", "b" })]
    [Arguments("a\r\nb", new[] { "a", "b" })]
    [Arguments("a\rb\r", new[] { "a", "b" })]
    [Arguments("\n\na", new[] { "", "", "a" })]
    [Arguments("a\r\n\r\nb", new[] { "a", "", "b" })]
    [Arguments("", new string[0])]
    public async Task Splits_Lines_Like_ReadLine(string output, string[] expectedLines)
    {
        var (target, buffer, lines) = CreateTarget();

        target.Append(output.AsSpan());
        target.Complete();

        await Assert.That(lines).IsEquivalentTo(expectedLines);
        await Assert.That(buffer.ToString()).IsEqualTo(output);
    }

    [Test]
    public async Task Joins_CarriageReturn_LineFeed_Split_Across_Chunks()
    {
        var (target, buffer, lines) = CreateTarget();

        target.Append("first\r".AsSpan());
        target.Append("\nsec".AsSpan());
        target.Append("ond".AsSpan());
        target.Complete();

        await Assert.That(lines).IsEquivalentTo(["first", "second"]);
        await Assert.That(buffer.ToString()).IsEqualTo("first\r\nsecond");
    }

    [Test]
    public async Task Captures_Into_Complete_Buffer_Without_Logger()
    {
        var buffer = new BoundedCommandOutputBuffer(0);
        var completeBuffer = new BoundedCommandOutputBuffer(0);
        var target = new CommandOutputTarget(buffer, completeBuffer, logLine: null);

        target.Append("a\nb".AsSpan());
        target.Complete();

        await Assert.That(completeBuffer.ToString()).IsEqualTo("a\nb");
    }

    [Test]
    [Arguments(65535, "")]
    [Arguments(65536, "")]
    [Arguments(65537, "")]
    [Arguments(65536, "\n")]
    [Arguments(65536, "\r\n")]
    [Arguments(65537, "\n")]
    public async Task Caps_Logged_Line_Without_Changing_Captured_Output(int length, string terminator)
    {
        var output = new string('x', length);
        var buffer = new BoundedCommandOutputBuffer(0);
        var completeBuffer = new BoundedCommandOutputBuffer(0);
        var lines = new List<string>();
        var target = new CommandOutputTarget(buffer, completeBuffer, lines.Add);

        target.Append((output + terminator).AsSpan());
        target.Complete();

        var expected = length > 65536 ? output[..65536] + " ... [line truncated]" : output;
        await Assert.That(lines).IsEquivalentTo([expected]);
        await Assert.That(buffer.ToString()).IsEqualTo(output + terminator);
        await Assert.That(completeBuffer.ToString()).IsEqualTo(output + terminator);
    }

    [Test]
    public async Task Flushes_At_Overflow_And_Discards_Remaining_Chunks_Until_Newline()
    {
        var buffer = new BoundedCommandOutputBuffer(32);
        var lines = new List<string>();
        var target = new CommandOutputTarget(buffer, completeOutputBuffer: null, lines.Add);
        var chunk = new string('x', 8192);

        for (var i = 0; i < 8; i++)
        {
            target.Append(chunk.AsSpan());
        }

        await Assert.That(lines).IsEmpty();
        target.Append("overflow".AsSpan());
        var expectedLine = new string('x', 65536) + " ... [line truncated]";
        await Assert.That(lines).IsEquivalentTo([expectedLine]);

        for (var i = 0; i < 1024; i++)
        {
            target.Append(chunk.AsSpan());
        }

        await Assert.That(lines).IsEquivalentTo([expectedLine]);

        target.Append("\nnext".AsSpan());
        target.Complete();
        await Assert.That(lines).IsEquivalentTo([expectedLine, "next"]);
    }

    [Test]
    [Arguments("\n", "")]
    [Arguments("\r", "\r")]
    [Arguments("\r", "\n")]
    public async Task Resumes_Logging_After_Truncated_Line_Terminator(string terminator, string nextChunkPrefix)
    {
        var (target, buffer, lines) = CreateTarget();
        var longLine = new string('x', 65537);
        var nextChunk = nextChunkPrefix + "\nnext\n" + longLine + terminator;

        target.Append((longLine + terminator).AsSpan());
        target.Append(nextChunk.AsSpan());
        target.Complete();

        var truncatedLine = new string('x', 65536) + " ... [line truncated]";
        await Assert.That(lines).IsEquivalentTo([truncatedLine, "", "next", truncatedLine]);
        await Assert.That(buffer.ToString()).IsEqualTo(longLine + terminator + nextChunk);
    }

    private static (CommandOutputTarget Target, BoundedCommandOutputBuffer Buffer, List<string> Lines) CreateTarget()
    {
        var buffer = new BoundedCommandOutputBuffer(0);
        var lines = new List<string>();
        return (new CommandOutputTarget(buffer, completeOutputBuffer: null, lines.Add), buffer, lines);
    }
}
