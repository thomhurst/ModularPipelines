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
    public async Task Sbom_Help_Exposes_Test_Subcommand()
    {
        const string helpText = """
            SBOM

            Usage
              snyk sbom --format=<FORMAT>
            """;

        var commands = new TestSnykCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["test"]);
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
    public async Task Known_Value_Options_Work_When_Help_Omits_Placeholders()
    {
        const string helpText = """
            Describe infrastructure.

            Options
              --packages-folder
                  Specify a custom path to the packages folder.
              --tfc-token
                  Specify an API token.
              --tfc-endpoint
                  Specify the Terraform Enterprise endpoint.
              --fetch-tfstate-headers
                  Use HTTP authorization headers when fetching Terraform state.
              --repo
                  Specify the repository URL.
            """;

        var command = await new TestSnykCliScraper().Parse(["snyk", "iac", "describe"], helpText);

        foreach (var switchName in new[]
                 {
                     "--packages-folder",
                     "--tfc-token",
                     "--tfc-endpoint",
                     "--fetch-tfstate-headers",
                     "--repo",
                 })
        {
            var option = command!.Options.Single(x => x.SwitchName == switchName);
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.IsFlag).IsFalse();
        }

        await Assert.That(command!.Options.Single(x => x.SwitchName == "--tfc-token").IsSecret).IsTrue();
        await Assert.That(command.Options.Single(x => x.SwitchName == "--fetch-tfstate-headers").IsSecret).IsTrue();
    }

    [Test]
    public async Task Sbom_Parses_Bracketed_Options_And_Adds_Documented_Flag()
    {
        const string helpText = """
            SBOM
            Usage: snyk sbom [<OPTIONS>]

            Options
              [--org=<ORG_ID>]
                Select an organization.
              [--dev]
                Include development dependencies.
              [--exclude=<NAME>]
                Exclude files or directories.
              [--detection-depth=<DEPTH>]
                Limit directory traversal.
              [--prune-repeated-subdependencies|-p]
                Prune repeated dependencies.
              [--json-file-output]
                Save JSON output to a file.
            """;

        var command = await new TestSnykCliScraper().Parse(["snyk", "sbom"], helpText);

        await Assert.That(command!.Options.Select(x => x.SwitchName)).Contains("--org");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--dev").IsFlag).IsTrue();
        await Assert.That(command.Options.Single(x => x.SwitchName == "--detection-depth").CSharpType).IsEqualTo("int?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--json-file-output").CSharpType).IsEqualTo("string?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--allow-incomplete-sbom").IsFlag).IsTrue();
    }

    [Test]
    public async Task Container_Sbom_Adds_Documented_Options_Missing_From_Older_Help()
    {
        var command = await new TestSnykCliScraper().Parse(
            ["snyk", "container", "sbom"],
            "Usage: snyk container sbom --format=<FORMAT> <IMAGE>");

        await Assert.That(command!.Options.Single(x => x.SwitchName == "--nested-jars-depth").CSharpType)
            .IsEqualTo("int?");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--exclude-node-modules").IsFlag).IsTrue();
        await Assert.That(command.Options.Single(x => x.SwitchName == "--password").IsSecret).IsTrue();
    }

    [Test]
    public async Task Sbom_Test_Parses_Required_File_Option()
    {
        const string helpText = """
            SBOM test
            Usage: snyk sbom test --file=<FILE_PATH> [<OPTIONS>]

            Options
              --file=<FILE_PATH>
                Required. Specify the SBOM document.
              --json
                Print JSON output.
            """;

        var command = await new TestSnykCliScraper().Parse(["snyk", "sbom", "test"], helpText);

        await Assert.That(command!.FullCommand).IsEqualTo("snyk sbom test");
        await Assert.That(command.Options.Single(x => x.SwitchName == "--file").IsRequired).IsTrue();
    }

    [Test]
    public async Task Code_Test_Models_Optional_Path()
    {
        var command = await new TestSnykCliScraper().Parse(
            ["snyk", "code", "test"],
            "Usage: snyk code test [<OPTIONS>] [<PATH>]");

        var path = command!.PositionalArguments.Single();
        await Assert.That(path.PropertyName).IsEqualTo("Path");
        await Assert.That(path.IsRequired).IsFalse();
    }

    [Test]
    public async Task Windows_Resolver_Finds_Standalone_Executable()
    {
        var root = Path.Combine(Path.GetTempPath(), "mp-snyk-resolver-tests", Guid.NewGuid().ToString("N"));

        try
        {
            Directory.CreateDirectory(root);
            var executable = Path.Combine(root, "snyk.exe");
            await File.WriteAllTextAsync(executable, string.Empty);

            var resolved = SnykCliScraper.ResolveExecutablePath(root, isWindows: true);

            await Assert.That(resolved).IsEqualTo(Path.GetFullPath(executable));
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
