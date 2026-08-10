using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class WinGetCliScraperTests
{
    [Test]
    public async Task Extracts_Subcommands_From_Hyphenated_Heading()
    {
        const string helpText = """
            usage: winget source [<command>] [<options>]

            The following sub-commands are available:
              add     Add a new source
              list    List current sources

            For more details on a specific command, pass it the help argument. [-?]
            """;

        var commands = new TestWinGetCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["add", "list"]);
    }

    [Test]
    public async Task Models_Repeatable_Sort_As_A_Collection()
    {
        const string helpText = """
            List installed packages.

            usage: winget list [<options>]

            The following options are available:
              --sort   Sort results by a property (can be repeated)
            """;

        var command = await new TestWinGetCliScraper().Parse(
            ["winget", "list"],
            helpText);
        var sort = command!.Options.Single(option => option.SwitchName == "--sort");

        using (Assert.Multiple())
        {
            await Assert.That(sort.AcceptsMultipleValues).IsTrue();
            await Assert.That(sort.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Does_Not_Mark_Boolean_Flags_As_Repeatable_Values()
    {
        const string helpText = """
            List installed packages.

            usage: winget list [<options>]

            The following options are available:
              --verbose   Enable verbose logging multiple times during troubleshooting
            """;

        var command = await new TestWinGetCliScraper().Parse(
            ["winget", "list"],
            helpText);
        var verbose = command!.Options.Single(option => option.SwitchName == "--verbose");

        using (Assert.Multiple())
        {
            await Assert.That(verbose.IsFlag).IsTrue();
            await Assert.That(verbose.AcceptsMultipleValues).IsFalse();
            await Assert.That(verbose.CSharpType).IsEqualTo("bool?");
        }
    }

    private sealed class TestWinGetCliScraper : WinGetCliScraper
    {
        public TestWinGetCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<WinGetCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) =>
            ExtractSubcommands(helpText).ToList();

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
