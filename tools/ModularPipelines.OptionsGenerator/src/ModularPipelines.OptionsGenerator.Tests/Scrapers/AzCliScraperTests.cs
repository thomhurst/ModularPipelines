using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AzCliScraperTests
{
    [Test]
    public async Task Boolean_Accepted_Values_Require_An_Explicit_Value()
    {
        const string helpText = """
            Command
                az eventhubs namespace create : Create an Event Hubs namespace.

            Optional Arguments
                --disable-local-auth : A boolean value that indicates whether SAS
                                       authentication is enabled/disabled for the
                                       Event Hubs. Allowed values: false, true.
                --force              : Force the operation without confirmation.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "eventhubs", "namespace", "create"],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--disable-local-auth");
        var force = command.Options.Single(item => item.SwitchName == "--force");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.ValueSeparator).IsEqualTo(" ");
            await Assert.That(force.IsFlag).IsTrue();
        }
    }

    [Test]
    public async Task Boolean_Lists_Remain_Collections()
    {
        const string helpText = """
            Command
                az vm application set : Set applications for a VM.

            Optional Arguments
                --treat-deployment-as-failure : Space-separated list of true or false corresponding
                                                to the application version ids.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "vm", "application", "set"],
            helpText);
        var option = command!.Options.Single(
            item => item.SwitchName == "--treat-deployment-as-failure");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
        }
    }

    private sealed class TestAzCliScraper()
        : AzCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AzCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}
