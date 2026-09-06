using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PackerCliScraperTests
{
    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "-only=foo,bar  to ..." line deliberately keeps two spaces so it satisfies
        // the option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage: packer build [options] TEMPLATE

            Options:
              -color=false                 Disable color output. (Default: color)
              -except=foo,bar,baz          Run all builds and post-processors other than these. Combine with
                                           -only=foo,bar  to narrow the selection further.
              -force                       Force a build to continue if artifacts exist, deletes existing artifacts.
            """;

        var command = await new TestPackerCliScraper().Parse(["packer", "build"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--color", "--except", "--force"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--except").Description)
                .IsEqualTo("Run all builds and post-processors other than these. Combine with -only=foo,bar  to narrow the selection further.");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--force").Description)
                .IsEqualTo("Force a build to continue if artifacts exist, deletes existing artifacts.");
        }
    }

    private sealed class TestPackerCliScraper()
        : PackerCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PackerCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
    }
}
