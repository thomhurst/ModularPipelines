using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PipCliScraperTests
{
    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "--no-deps  to ..." line deliberately keeps two spaces so it satisfies the
        // option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage:
              pip install [options] <requirement specifier> ...

            Install Options:
              -r, --requirement <file>    Install from the given requirements file. Combine with
                                          --no-deps  to skip dependency installation.
              -e, --editable <path/url>   Install a project in editable mode.
            """;

        var command = await new TestPipCliScraper().Parse(["pip", "install"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--requirement", "--editable"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--requirement").Description)
                .IsEqualTo("Install from the given requirements file. Combine with --no-deps  to skip dependency installation.");
        }
    }

    private sealed class TestPipCliScraper()
        : PipCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PipCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }
}
