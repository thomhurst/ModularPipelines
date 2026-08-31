using ModularPipelines.Context;
using ModularPipelines.GitHub.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.GitHub.UnitTests;

public class GhCodespaceCpOptionsTests : TestBase
{
    [Test]
    public async Task Renders_Scp_Flags_Between_Terminator_And_Operands()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new GhCodespaceCpOptions(["source"], "destination")
        {
            Recursive = true,
            ScpFlags = ["-F", "config"],
        });

        await Assert.That(commandLine.ToString())
            .IsEqualTo("gh codespace cp --recursive -- -F config source destination");
    }

    [Test]
    public async Task Omits_Terminator_Without_Scp_Flags()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(
            new GhCodespaceCpOptions(["source"], "destination"));

        await Assert.That(commandLine.ToString())
            .IsEqualTo("gh codespace cp source destination");
    }
}
