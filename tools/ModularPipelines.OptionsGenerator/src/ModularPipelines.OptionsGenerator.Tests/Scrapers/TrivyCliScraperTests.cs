using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class TrivyCliScraperTests
{
    [Test]
    public async Task Root_Help_Parses_Command_Sections_Without_Colons()
    {
        const string helpText = """
            Usage:
              trivy [command]

            Scanning Commands
              config      Scan config files
              image       Scan a container image

            Management Commands
              module      Manage modules

            Utility Commands
              convert     Convert a report

            Flags:
              -h, --help  help for trivy
            """;

        var commands = new TestTrivyCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["config", "image", "module", "convert"]);
    }

    [Test]
    public async Task Root_Help_Ignores_Command_Descriptions_That_End_In_Command()
    {
        const string helpText = """
            Examples:
              help        Help about any command
              phantom     This is not a command entry

            Available Commands:
              image       Scan a container image

            Flags:
              -h, --help  help for trivy
            """;

        var commands = new TestTrivyCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["image"]);
    }

    [Test]
    public async Task Clean_Command_Is_Not_Skipped()
    {
        var scraper = new TestTrivyCliScraper();

        await Assert.That(scraper.Skips("clean")).IsFalse();
    }

    [Test]
    public async Task Cache_Default_Is_Normalized_Across_Platforms()
    {
        var scraper = new TestTrivyCliScraper();
        string[] descriptions =
        [
            @"cache directory (default ""C:\Users\runneradmin\AppData\Local\trivy"")",
            "cache directory (default \"/home/runner/.cache/trivy\")",
            "cache directory (default \"/Users/runner/Library/Caches/trivy\")",
        ];

        foreach (var description in descriptions)
        {
            await Assert.That(scraper.NormalizeDescription(description))
                .IsEqualTo("cache directory (default \"<cache>/trivy\")");
        }
    }

    [Test]
    public async Task Home_Directory_Path_Is_Normalized_Across_Platforms()
    {
        var scraper = new TestTrivyCliScraper();

        var description = scraper.NormalizeDescription(
            @"module directory (default ""C:\Users\runneradmin\.trivy\modules"")");

        await Assert.That(description)
            .IsEqualTo("module directory (default \"<home>/.trivy/modules\")");
    }

    [Test]
    public async Task Image_Help_Parses_Target_Types_And_Secrets()
    {
        const string helpText = """
            Scan a container image

            Usage:
              trivy image [flags] IMAGE_NAME

            Image Flags
                  --format string       report format
                  --severity strings    severities of security issues
                                       Allowed values:
                                         - UNKNOWN
                                         - LOW
                                         - MEDIUM
                                         - HIGH
                                         - CRITICAL

            Client/Server Flags
                  --timeout duration    timeout
                  --username string     registry username
                  --password string     registry password
            """;

        var command = await new TestTrivyCliScraper().Parse(["trivy", "image"], helpText);

        await Assert.That(command!.PositionalArguments.Single().PropertyName).IsEqualTo("ImageName");
        await Assert.That(command.PositionalArguments.Single().IsRequired).IsFalse();
        await Assert.That(command.PositionalArguments.Single().CSharpType).IsEqualTo("string?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--severity").CSharpType)
            .IsEqualTo("IEnumerable<TrivyImageSeverity>?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--timeout").CSharpType)
            .IsEqualTo("string?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--password").IsSecret).IsTrue();
    }

    [Test]
    public async Task Secret_Config_Path_Is_Not_Classified_As_Secret()
    {
        const string helpText = """
            Scan a container image

            Usage:
              trivy image [flags] IMAGE_NAME

            Secret Flags
                  --secret-config string   specify a path to config file for secret scanning
            """;

        var command = await new TestTrivyCliScraper().Parse(["trivy", "image"], helpText);
        var secretConfig = command!.Options.Single(x => x.SwitchName == "--secret-config");

        await Assert.That(secretConfig.IsSecret).IsFalse();
    }

    [Test]
    public async Task Plugin_Upgrade_Accepts_Multiple_Optional_Names()
    {
        const string helpText = """
            Upgrade installed plugins to newer versions

            Usage:
              trivy plugin upgrade [PLUGIN_NAMES]

            Flags:
              -h, --help   help for upgrade
            """;

        var command = await new TestTrivyCliScraper().Parse(["trivy", "plugin", "upgrade"], helpText);
        var positional = command!.PositionalArguments.Single();

        await Assert.That(positional.PropertyName).IsEqualTo("PluginNames");
        await Assert.That(positional.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(positional.IsRequired).IsFalse();
    }

    [Test]
    public async Task Plugin_Run_Accepts_Trailing_Plugin_Arguments()
    {
        const string helpText = """
            Run a plugin on the fly

            Usage:
              trivy plugin run [flags] NAME | URL | FILE_PATH

            Flags:
              -h, --help   help for run
            """;

        var command = await new TestTrivyCliScraper().Parse(["trivy", "plugin", "run"], helpText);
        var positionals = command!.PositionalArguments;

        await Assert.That(positionals).Count().IsEqualTo(2);
        await Assert.That(positionals[0].PropertyName).IsEqualTo("Source");
        await Assert.That(positionals[0].IsRequired).IsTrue();
        await Assert.That(positionals[1].PropertyName).IsEqualTo("PluginArguments");
        await Assert.That(positionals[1].CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(positionals[1].IsRequired).IsFalse();
        await Assert.That(positionals[1].PositionIndex).IsEqualTo(0);
        await Assert.That(positionals[1].Phase).IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    public async Task Vex_Repo_Download_Accepts_Multiple_Optional_Names()
    {
        const string helpText = """
            Download VEX repositories

            Usage:
              trivy vex repo download [REPO_NAMES] [flags]

            Flags:
              -h, --help   help for download
            """;

        var command = await new TestTrivyCliScraper().Parse(["trivy", "vex", "repo", "download"], helpText);
        var positional = command!.PositionalArguments.Single();

        await Assert.That(positional.PropertyName).IsEqualTo("RepoNames");
        await Assert.That(positional.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(positional.IsRequired).IsFalse();
    }

    [Test]
    public async Task Vex_Command_Preserves_Optional_Command_Operand()
    {
        const string helpText = """
            [EXPERIMENTAL] VEX utilities

            Usage:
              trivy vex [command]

            Available Commands:
              repo        Manage VEX repositories
            """;

        var command = await new TestTrivyCliScraper().Parse(["trivy", "vex"], helpText);
        var positional = command!.PositionalArguments.Single();

        await Assert.That(positional.PropertyName).IsEqualTo("Command");
        await Assert.That(positional.CSharpType).IsEqualTo("string?");
        await Assert.That(positional.IsRequired).IsFalse();
    }

    private sealed class TestTrivyCliScraper : TrivyCliScraper
    {
        public TestTrivyCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<TrivyCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();

        public bool Skips(string subcommand) => IsSkippableSubcommand(subcommand);

        public string NormalizeDescription(string description) => NormalizeOptionDescription(description);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
