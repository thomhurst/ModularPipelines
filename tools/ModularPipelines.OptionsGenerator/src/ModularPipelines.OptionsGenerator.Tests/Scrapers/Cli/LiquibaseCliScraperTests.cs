using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class LiquibaseCliScraperTests
{
    private readonly TestLiquibaseCliScraper _scraper = new();

    [Test]
    public async Task ExtractSubcommands_Parses_Current_Header_And_Aliases()
    {
        const string helpText = """
            Commands
              update                Deploy changes
              init                  Initialize Liquibase
              project, project-new  Creates a project

            Each argument can be specified using environment variables.
            """;

        var commands = _scraper.ParseSubcommands(helpText).ToArray();

        await Assert.That(commands).IsEquivalentTo(["update", "init", "project"]);
    }

    [Test]
    public async Task ParseOptions_Models_Current_Picocli_Help()
    {
        const string helpText = """
              -D=PARAM                     Pass a name/value pair for substitution
              -h, --help                   Show this help message and exit
                  --changelog-file=PARAM   Changelog path [REQUIRED]
                  --count=PARAM            Number of changesets
                  --password=PARAM         Database password
                  --show-summary=PARAM     Values can be 'off', 'summary', or 'verbose'. DEFAULT: SUMMARY
                  --use-legacy-checksum=PARAM Use the legacy checksum
                                           DEFAULT: false
                                           (defaults file: 'liquibase.command.useLegacyChecksum')
            """;

        var options = _scraper.ParseLiquibaseOptions(helpText);

        var define = options.Single(x => x.SwitchName == "-D");
        await Assert.That(define.CSharpType).IsEqualTo("IEnumerable<KeyValue>?");
        await Assert.That(define.AcceptsMultipleValues).IsTrue();
        await Assert.That(define.IsKeyValue).IsTrue();
        await Assert.That(define.ValueSeparator).IsEqualTo(string.Empty);

        var help = options.Single(x => x.SwitchName == "--help");
        await Assert.That(help.IsFlag).IsTrue();
        await Assert.That(help.ShortForm).IsEqualTo("-h");

        var changelogFile = options.Single(x => x.SwitchName == "--changelog-file");
        await Assert.That(changelogFile.IsRequired).IsTrue();
        await Assert.That(changelogFile.CSharpType).IsEqualTo("string?");
        await Assert.That(options.Single(x => x.SwitchName == "--count").CSharpType).IsEqualTo("int?");
        await Assert.That(options.Single(x => x.SwitchName == "--password").IsSecret).IsTrue();
        await Assert.That(options.Single(x => x.SwitchName == "--show-summary").EnumDefinition).IsNotNull();

        var legacyChecksum = options.Single(x => x.SwitchName == "--use-legacy-checksum");
        await Assert.That(legacyChecksum.IsFlag).IsFalse();
        await Assert.That(legacyChecksum.CSharpType).IsEqualTo("bool?");
        await Assert.That(legacyChecksum.Description).IsEqualTo("Use the legacy checksum DEFAULT: false");
    }

    [Test]
    public async Task ParseCommand_Preserves_Multiline_Description()
    {
        const string helpText = """
            Outputs a description of differences. If you have a Liquibase License Key, you
            can output the differences as JSON using the --format=JSON option

            Usage: liquibase diff [OPTIONS]
            """;

        var command = await _scraper.ParseLiquibaseCommand(["liquibase", "diff"], helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Description).IsEqualTo(
            "Outputs a description of differences. If you have a Liquibase License Key, you can output the differences as JSON using the --format=JSON option");
    }

    [Test]
    public async Task Scrape_Separates_Global_Options_From_Command_Options()
    {
        const string rootHelp = """
            Usage: liquibase [OPTIONS] [COMMAND]

            Commands
              update                Deploy changes

            Global Options:
                  --search-path=PARAM   Paths to search for changelogs
                  --log-level=PARAM     Logging level
            """;
        const string updateHelp = """
            Usage: liquibase update [OPTIONS]

                  --search-path=PARAM      Paths to search for changelogs
                  --changelog-file=PARAM  Changelog path [REQUIRED]
            """;
        var scraper = new TestLiquibaseCliScraper(new StubExecutor(rootHelp, updateHelp));

        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var tool = scraper.CreateToolDefinition();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName))
            .IsEquivalentTo(["--search-path", "--log-level"]);
        await Assert.That(commands.Count).IsEqualTo(1);
        await Assert.That(commands[0].Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--changelog-file"]);
    }

    private sealed class TestLiquibaseCliScraper(ICliCommandExecutor? executor = null)
        : LiquibaseCliScraper(executor ?? new StubExecutor(), new StubHelpTextCache(), NullLogger<LiquibaseCliScraper>.Instance)
    {
        public IEnumerable<string> ParseSubcommands(string helpText) => ExtractSubcommands(helpText);

        public List<CliOptionDefinition> ParseLiquibaseOptions(string helpText) => ParseOptions(helpText);

        public Task<CliCommandDefinition?> ParseLiquibaseCommand(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }

    private sealed class StubExecutor(string? rootHelp = null, string? updateHelp = null) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) => Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "--help" ? rootHelp ?? string.Empty : updateHelp ?? string.Empty,
                StandardError = string.Empty,
                ExitCode = rootHelp is null ? -1 : 0,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(rootHelp is not null);
    }

    private sealed class StubHelpTextCache : IHelpTextCache
    {
        public bool TryGet(string cacheKey, out string? helpText)
        {
            helpText = null;
            return false;
        }

        public void Set(string cacheKey, string helpText)
        {
        }

        public void Clear()
        {
        }

        public CacheStatistics GetStatistics() => new();
    }
}
