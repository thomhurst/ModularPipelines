using Microsoft.Extensions.Logging.Abstractions;
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

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}
