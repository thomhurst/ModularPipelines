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
        await Assert.That(changelogFile.IsRequired).IsFalse();
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
    public async Task Monitor_Performance_Accepts_Boolean_Or_Filename_Values()
    {
        const string helpText = """
                  --monitor-performance=PARAM   Enable performance monitoring. DEFAULT: false
            """;

        var option = _scraper.ParseLiquibaseOptions(helpText).Single();

        await Assert.That(option.SwitchName).IsEqualTo("--monitor-performance");
        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    public async Task Prompt_For_Non_Local_Database_Is_Boolean()
    {
        const string helpText = """
                  --prompt-for-non-local-database=PARAM   Prompt before running against a non-local database
            """;

        var option = _scraper.ParseLiquibaseOptions(helpText).Single();

        await Assert.That(option.SwitchName).IsEqualTo("--prompt-for-non-local-database");
        await Assert.That(option.CSharpType).IsEqualTo("bool?");
        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    public async Task Global_Options_Include_Documented_Databricks_Diff_Filters()
    {
        const string helpText = """
                  --databricks-diff-tblproperties-exclude-patterns=PARAM   Excluded TBLPROPERTIES
            """;
        var options = _scraper.ParseLiquibaseGlobalOptions(helpText);

        var excludePatterns = options.Single(
            option => option.SwitchName == "--databricks-diff-tblproperties-exclude-patterns");
        await Assert.That(excludePatterns.PropertyName).IsEqualTo("DatabricksDiffTblPropertiesExcludePatterns");
        await Assert.That(excludePatterns.CSharpType).IsEqualTo("string?");
        await Assert.That(excludePatterns.IsFlag).IsFalse();
        await Assert.That(excludePatterns.Description).IsEqualTo("Excluded TBLPROPERTIES");

        var ignoreAll = options.Single(
            option => option.SwitchName == "--databricks-diff-tblproperties-ignore-all");
        await Assert.That(ignoreAll.PropertyName).IsEqualTo("DatabricksDiffTblPropertiesIgnoreAll");
        await Assert.That(ignoreAll.CSharpType).IsEqualTo("bool?");
        await Assert.That(ignoreAll.IsFlag).IsFalse();
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
    public async Task ParseCommand_Adds_Documented_Diff_Format_Option()
    {
        const string helpText = "Usage: liquibase diff [OPTIONS]";

        var command = await _scraper.ParseLiquibaseCommand(["liquibase", "diff"], helpText);

        var format = command!.Options.Single(option => option.SwitchName == "--format");
        await Assert.That(format.PropertyName).IsEqualTo("Format");
        await Assert.That(format.CSharpType).IsEqualTo("string?");
        await Assert.That(format.ValueSeparator).IsEqualTo("=");
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
            .IsEquivalentTo(
            [
                "--search-path",
                "--log-level",
                "--databricks-diff-tblproperties-exclude-patterns",
                "--databricks-diff-tblproperties-ignore-all",
            ]);
        await Assert.That(commands.Count).IsEqualTo(1);
        await Assert.That(commands[0].Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--changelog-file"]);
    }

    [Test]
    public async Task Windows_Resolver_Prefers_Batch_Launcher()
    {
        var root = Path.Combine(Path.GetTempPath(), "mp-liquibase-resolver-tests", Guid.NewGuid().ToString("N"));

        try
        {
            Directory.CreateDirectory(root);
            var batchLauncher = Path.Combine(root, "liquibase.bat");
            await File.WriteAllTextAsync(batchLauncher, string.Empty);

            var resolved = LiquibaseCliScraper.ResolveExecutablePath(root, isWindows: true);

            await Assert.That(resolved).IsEqualTo(Path.GetFullPath(batchLauncher));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Test]
    public async Task Windows_Resolver_Falls_Back_To_Normal_Command_Name()
    {
        var resolved = LiquibaseCliScraper.ResolveExecutablePath(string.Empty, isWindows: true);

        await Assert.That(resolved).IsEqualTo("liquibase");
    }

    private sealed class TestLiquibaseCliScraper(ICliCommandExecutor? executor = null)
        : LiquibaseCliScraper(executor ?? new StubExecutor(), new StubHelpTextCache(), NullLogger<LiquibaseCliScraper>.Instance)
    {
        public IEnumerable<string> ParseSubcommands(string helpText) => ExtractSubcommands(helpText);

        public List<CliOptionDefinition> ParseLiquibaseOptions(string helpText) => ParseOptions(helpText);

        public IReadOnlyList<CliOptionDefinition> ParseLiquibaseGlobalOptions(string helpText) =>
            ParseGlobalOptions(helpText);

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
