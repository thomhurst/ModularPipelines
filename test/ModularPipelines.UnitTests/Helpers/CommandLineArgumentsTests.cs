using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandLineArgumentsTests
{
    [Test]
    [Arguments("plain", "plain")]
    [Arguments("", "\"\"")]
    [Arguments("with space", "\"with space\"")]
    [Arguments("tab\there", "\"tab\there\"")]
    [Arguments("say \"hi\"", "\"say \\\"hi\\\"\"")]
    [Arguments(@"C:\path\", @"C:\path\")]
    [Arguments(@"C:\my path\", @"""C:\my path\\""")]
    [Arguments(@"a\""b", @"""a\\\""b""")]
    [Arguments(@"a\\b c", @"""a\\b c""")]
    public async Task Escape_Follows_Windows_Command_Line_Rules(string argument, string expected)
    {
        await Assert.That(CommandLineArguments.Escape(argument)).IsEqualTo(expected);
    }

    [Test]
    public async Task Join_Separates_Escaped_Arguments_With_Spaces()
    {
        await Assert.That(CommandLineArguments.Join(["build", "my project", ""]))
            .IsEqualTo("build \"my project\" \"\"");
    }
}
