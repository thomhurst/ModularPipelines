using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Helpers;

public class ShellArgumentEscaperTests
{
    [Test]
    public async Task EscapesEmbeddedSingleQuotes()
    {
        var escaped = ShellArgumentEscaper.Escape("/tmp/it's ready.sh");

        await Assert.That(escaped).IsEqualTo("'/tmp/it'\\''s ready.sh'");
    }
}
