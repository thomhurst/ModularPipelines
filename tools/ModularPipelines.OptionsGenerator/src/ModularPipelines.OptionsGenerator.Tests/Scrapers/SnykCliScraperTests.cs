using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SnykCliScraperTests
{
    [Test]
    public async Task Root_Help_Extracts_Only_Top_Level_Commands()
    {
        const string helpText = """
            Available commands
              snyk auth
              snyk test
              snyk aibom

            See also: snyk aibom test — Test an AI-BOM document.
            """;

        var commands = new TestSnykCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["auth", "test", "aibom"]);
    }

    [Test]
    public async Task Group_Help_Extracts_Only_That_Groups_Commands()
    {
        const string helpText = """
            snyk container commands and the help docs
              - container test, test an image
              - container monitor, monitor an image
              - container sbom, create an SBOM
              - iac test, unrelated example
            """;

        var commands = new TestSnykCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["test", "monitor", "sbom"]);
    }

    [Test]
    public async Task Command_Help_Does_Not_Treat_Examples_As_Subcommands()
    {
        const string helpText = """
            Test a project for vulnerabilities.

            Usage
              snyk test [<OPTIONS>]

            See code test, container test, and iac test for related commands.
            """;

        await Assert.That(new TestSnykCliScraper().Extract(helpText)).IsEmpty();
    }

    [Test]
    public async Task Auth_Parses_Secret_Positional_And_Combined_Options()
    {
        const string helpText = """
            Authenticate Snyk CLI.

            Usage
              snyk auth [<API_TOKEN>] [<OPTIONS>]

            Options
              --auth-type=<TYPE>
                  Authentication type.
              --client-secret=<SECRET> and --client-id=<ID>
                  OAuth client credentials.
            """;

        var command = await new TestSnykCliScraper().Parse(["snyk", "auth"], helpText);

        var argument = command!.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("ApiToken");
        await Assert.That(argument.IsRequired).IsFalse();
        await Assert.That(argument.IsSecret).IsTrue();
        await Assert.That(command.Options.Select(x => x.SwitchName))
            .IsEquivalentTo(["--auth-type", "--client-secret", "--client-id"]);
        await Assert.That(command.Options.Single(x => x.SwitchName == "--client-secret").IsSecret).IsTrue();
        await Assert.That(command.Options.Single(x => x.SwitchName == "--client-id").IsSecret).IsFalse();
    }

    [Test]
    public async Task Value_Hints_Detect_Boolean_Enum_And_Integer_Types()
    {
        const string helpText = """
            Test a project.

            Options
              --reachability=<true|false>
                  Analyze reachability.
              --severity-threshold=<low|medium|high|critical>
                  Minimum severity.
              --detection-depth=<DEPTH>
                  Dependency discovery depth.
              --max-depth
                  Maximum dependency depth.
            """;

        var command = await new TestSnykCliScraper().Parse(["snyk", "test"], helpText);

        await Assert.That(command!.Options.Single(x => x.SwitchName == "--reachability").CSharpType)
            .IsEqualTo("bool?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--reachability").EnumDefinition)
            .IsNull();
        await Assert.That(command.Options.Single(x => x.SwitchName == "--severity-threshold").CSharpType)
            .IsEqualTo("SnykSeverityThreshold?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--detection-depth").CSharpType)
            .IsEqualTo("int?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--max-depth").CSharpType)
            .IsEqualTo("int?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--max-depth").IsFlag)
            .IsFalse();
    }

    [Test]
    public async Task Policy_With_Only_Positional_And_Debug_Is_Generated()
    {
        const string helpText = """
            Policy
            Usage
              snyk policy [<PATH_TO_POLICY_FILE>] [<OPTIONS>]

            Debug
              Use the -d option to output the debug logs.
            """;

        var scraper = new TestSnykCliScraper();
        var command = await scraper.Parse(["snyk", "policy"], helpText);

        await Assert.That(scraper.CanGenerate(helpText)).IsTrue();
        await Assert.That(command!.PositionalArguments.Single().PropertyName).IsEqualTo("PathToPolicyFile");
        await Assert.That(command.Options.Single().SwitchName).IsEqualTo("-d");
        await Assert.That(command.Options.Single().IsFlag).IsTrue();
    }

    private sealed class TestSnykCliScraper : SnykCliScraper
    {
        public TestSnykCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<SnykCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();

        public bool CanGenerate(string helpText) => HasOptions(helpText);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}
