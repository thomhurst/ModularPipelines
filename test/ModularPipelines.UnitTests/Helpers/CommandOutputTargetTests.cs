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

    private static (CommandOutputTarget Target, BoundedCommandOutputBuffer Buffer, List<string> Lines) CreateTarget()
    {
        var buffer = new BoundedCommandOutputBuffer(0);
        var lines = new List<string>();
        return (new CommandOutputTarget(buffer, completeOutputBuffer: null, lines.Add), buffer, lines);
    }
}
