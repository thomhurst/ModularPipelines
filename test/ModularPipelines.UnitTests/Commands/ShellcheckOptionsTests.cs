using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Shellcheck.Enums;
using ModularPipelines.Shellcheck.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Commands;

public class ShellcheckOptionsTests : TestBase
{
    [Test]
    public async Task Minimal_Options_Parse_As_Expected()
    {
        var result = await GetResult(new ShellcheckExecuteOptions(["script.sh"]));

        await Assert.That(result.CommandInput).IsEqualTo("shellcheck script.sh");
    }

    [Test]
    public async Task Typed_Options_Parse_As_Expected()
    {
        var result = await GetResult(new ShellcheckExecuteOptions(["script.sh", "lib.sh"])
        {
            CheckSourced = true,
            Color = ShellcheckColor.Always,
            Include = "SC1000,SC1001",
            ExtendedAnalysis = false,
            Format = ShellcheckFormat.Json1,
            Shell = ShellcheckShell.Busybox,
            Severity = ShellcheckSeverity.Warning,
            WikiLinkCount = 2,
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "shellcheck --check-sourced --color=always --include=SC1000,SC1001 --extended-analysis=false --format=json1 --shell=busybox --severity=warning --wiki-link-count=2 script.sh lib.sh");
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommand>();
        return await command.ExecuteCommandLineTool(options, new CommandExecutionOptions { InternalDryRun = true });
    }
}
