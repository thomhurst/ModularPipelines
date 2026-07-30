using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class BuildSystemFormatterExtensionsTests
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task WriteGroupCommand_Routes_To_Expected_Writer(bool usesRawCommands)
    {
        const string command = "[red]::group::dynamic[name][/]";
        var rawWrites = new List<string>();
        var fallbackWrites = new List<string>();
        var formatter = new TestFormatter(usesRawCommands);

        formatter.WriteGroupCommand(command, rawWrites.Add, fallbackWrites.Add);

        var expectedRawWrites = usesRawCommands ? new[] { command } : [];
        var expectedFallbackWrites = usesRawCommands ? [] : new[] { command };

        await Assert.That(rawWrites).IsEquivalentTo(expectedRawWrites);
        await Assert.That(fallbackWrites).IsEquivalentTo(expectedFallbackWrites);
    }

    private sealed class TestFormatter(bool usesRawCommands) : IBuildSystemFormatter
    {
        public bool UsesRawCommands { get; } = usesRawCommands;

        public string? GetStartBlockCommand(string name) => null;

        public string? GetEndBlockCommand(string name) => null;

        public string? GetMaskSecretCommand(string secret) => null;
    }
}
