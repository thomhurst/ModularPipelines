using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PnpmCliScraperTests
{
    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "--save-exact  to ..." line deliberately keeps two spaces so it satisfies
        // the option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage: pnpm add <name>

            Options:
              -D, --save-dev                 Save package to your `devDependencies`. Combine with
                                             --save-exact  to pin the installed version.
              -E, --save-exact               Install exact version
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "add"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--save-dev", "--save-exact"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--save-dev").Description)
                .IsEqualTo("Save package to your `devDependencies`. Combine with --save-exact  to pin the installed version.");
        }
    }

    private sealed class TestPnpmCliScraper()
        : PnpmCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PnpmCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }
}
