using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SnykCliScraperTests
{
    [Test]
    [Arguments("(json|yaml)")]
    [Arguments("{json|yaml}")]
    [Arguments("<json|yaml>")]
    public async Task Choice_Hints_Use_Shared_Wrapper_Parsing(string hint)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"],
            $"Options\n  --mode={hint}\n    Select mode."))!;
        await Assert.That(command.Options.Single().EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["json", "yaml"]);
    }

    [Test]
    [Arguments("(true|false)")]
    [Arguments("{true|false}")]
    [Arguments("<true|false>")]
    public async Task Wrapped_Boolean_Choices_Remain_Value_Options(string hint)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"],
            $"Options\n  --reachable={hint}\n    Select reachability."))!;
        var option = command.Options.Single();
        await Assert.That(option.CSharpType).IsEqualTo("bool?");
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.EnumDefinition).IsNull();
    }

    [Test]
    [Arguments("(json|yaml")]
    [Arguments("json|yaml)")]
    [Arguments("json||yaml")]
    [Arguments("true||false")]
    [Arguments("(json||yaml)")]
    [Arguments("()")]
    [Arguments("[]")]
    [Arguments("{}")]
    [Arguments("<>")]
    [Arguments("<json|yaml")]
    [Arguments("json|yaml>")]
    [Arguments("<json|yaml)")]
    public async Task Malformed_Choice_Hints_Remain_In_String_Descriptions(string hint)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"],
            $"Options\n  --mode={hint}\n    Select mode."))!;
        var option = command.Options.Single();
        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.ValueSeparator).IsEqualTo("=");
        await Assert.That(option.EnumDefinition).IsNull();
        await Assert.That(option.Description).Contains(hint);
    }

    [Test]
    [Arguments("Required. Specify the identifier.", true)]
    [Arguments("Required: specify the identifier.", true)]
    [Arguments("The identifier is otherwise required.", false)]
    [Arguments("Not required.", false)]
    [Arguments("Required when no file path is provided.", false)]
    [Arguments("Required: when no file path is supplied.", false)]
    [Arguments("Required. Only if no file path is supplied.", false)]
    [Arguments("Required: unless a file path is supplied.", false)]
    [Arguments("Required: Conditional.", false)]
    [Arguments("Required: No.", false)]
    [Arguments("Required: false.", false)]
    [Arguments("Required: optional.", false)]
    [Arguments("Required. Conditional.", false)]
    [Arguments("Required. No.", false)]
    [Arguments("Required. false.", false)]
    [Arguments("Required. optional.", false)]
    [Arguments("Required: Yes.", true)]
    [Arguments("Required: true.", true)]
    public async Task Only_Unconditional_Required_Markers_Require_Options(string description, bool required)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "ignore"], $"Options\n  --id=<ISSUE_ID>\n    {description}"))!;
        await Assert.That(command.Options.Single(option => option.SwitchName == "--id").IsRequired).IsEqualTo(required);
    }

    [Test]
    public async Task Short_Explicit_Description_Is_Preserved()
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"], "Description\n  Scan the project.\n\nOptions\n  --json\n    Emit JSON."))!;
        await Assert.That(command.Description).IsEqualTo("Scan the project.");
    }

    [Test]
    [Arguments("Test a project.\n\n", "Test a project.")]
    [Arguments("", null)]
    [Arguments("Test\n", null)]
    public async Task Fallback_Description_Stops_Before_Options(string introduction, string? expected)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"],
            introduction + "Options\n  --severity-threshold=<low|medium|high|critical>\n    Choose the minimum severity."))!;
        await Assert.That(command.Description).IsEqualTo(expected);
    }

    [Test]
    [Arguments("", null)]
    [Arguments("Test a project.\n", "Test a project.")]
    public async Task Colon_Options_Heading_Stops_Fallback_Description(string introduction, string? expected)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"],
            introduction + "Options:\n  --json\n    Emit JSON."))!;
        await Assert.That(command.Description).IsEqualTo(expected);
    }

    [Test]
    [Arguments("Options:")]
    [Arguments("Usage:")]
    [Arguments("Examples:")]
    [Arguments("Prerequisites:")]
    [Arguments("Debug:")]
    [Arguments("Exit codes:")]
    [Arguments("Environment variables:")]
    public async Task Colon_Section_Headings_End_Description(string heading)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"],
            $"Description\nTest a project.\n{heading}\nUnrelated section content."))!;
        await Assert.That(command.Description).IsEqualTo("Test a project.");
    }

    [Test]
    [Arguments("Options for authentication include OAuth and API tokens.")]
    [Arguments("Usage of this flag disables auto-detection.")]
    [Arguments("Examples below show how to scan a project.")]
    public async Task Heading_Words_In_Description_Prose_Are_Preserved(string prose)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"],
            $"Description\n{prose}\nAdditional details remain in the summary.\nOptions:\n  --json\n    Emit JSON."))!;
        await Assert.That(command.Description).IsEqualTo(prose + " Additional details remain in the summary.");
    }

    [Test]
    [Arguments("Usage: snyk test [<OPTIONS>]")]
    [Arguments("Usage: $ snyk test [<OPTIONS>]")]
    public async Task Inline_Usage_Is_Not_A_Description(string usage)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "test"], usage))!;
        await Assert.That(command.Description).IsNull();
    }

    [Test]
    [Arguments("")]
    [Arguments(":")]
    public async Task Ignore_Uses_The_Complete_Description_And_Conditional_Id(string headingSuffix)
    {
        var help = $"""
            Ignore
            Usage and description{headingSuffix}
              Ignore
                snyk ignore --id=<ISSUE_ID> [OPTIONS]

                The snyk ignore command modifies the .snyk policy file to ignore a specified issue according to
                its Snyk ID for all occurrences, its expiry date, a reason, or according to paths in the
                filesystem for the policy, the issue, or both.

              Exclude
                snyk ignore [--file-path=<PATH_TO_RESOURCE>] [OPTIONS]

                You can exclude directories or files from scanning using the --file-path option.

            Options
              --id=<ISSUE_ID>
                Snyk ID for the issue to ignore, omitted if the ignore command used with --file-path, otherwise required.
              --file-path=<PATH_TO_RESOURCE>
                Filesystem for which to exclude directories or files from scanning.
            """;

        var command = (await new TestSnykCliScraper().Parse(["snyk", "ignore"], help))!;
        await Assert.That(command.Description).IsEqualTo("The snyk ignore command modifies the .snyk policy file to ignore a specified issue according to its Snyk ID for all occurrences, its expiry date, a reason, or according to paths in the filesystem for the policy, the issue, or both.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--id").IsRequired).IsFalse();
        var group = command.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsTrue();
        await Assert.That(group.IsMutuallyExclusive).IsFalse();
        await Assert.That(group.PropertyNames).IsEquivalentTo(["Id", "FilePath"]);
        await Assert.That(group.Members.Select(member => member.OptionSwitch!))
            .IsEquivalentTo(["--id", "--file-path"]);
    }

    [Test]
    [Arguments("omitted if the command is used with --credential-file, otherwise required.", "--credential-file", true)]
    [Arguments("OMITTED IF used with --credential-file, OTHERWISE REQUIRED.", "--credential-file", true)]
    [Arguments("omitted if used with --credential-file, otherwise optional.", "--credential-file", false)]
    [Arguments("omitted if used with --credential-file.", "--credential-file", false)]
    [Arguments("omitted if used with --missing, otherwise required.", "--credential-file", false)]
    [Arguments("omitted if used with --token, otherwise required.", "--credential-file", false)]
    public async Task Conditional_Required_Alternatives_Resolve_Documented_Switches(
        string description, string alternativeSwitch, bool hasGroup)
    {
        var command = (await new TestSnykCliScraper().Parse(["snyk", "auth"],
            $"Options\n  --token=<TOKEN>\n    {description}\n  {alternativeSwitch}=<PATH>\n    Read credentials from this file."))!;

        await Assert.That(command.RequiredAlternativeGroups.Count).IsEqualTo(hasGroup ? 1 : 0);
        if (hasGroup)
        {
            var group = command.RequiredAlternativeGroups.Single();
            await Assert.That(group.PropertyNames).IsEquivalentTo(["Token", "CredentialFile"]);
            await Assert.That(group.IsRequired).IsTrue();
            await Assert.That(group.IsMutuallyExclusive).IsFalse();
            await Assert.That(command.Options.All(option => !option.IsRequired)).IsTrue();
        }
    }

    [Test]
    [Arguments("Monitor")]
    [Arguments("Container monitor")]
    [Arguments("Usage:")]
    [Arguments("Monitor a container image")]
    public async Task Combined_Description_Skips_Headings_Before_Synopsis(string heading)
    {
        var help = $"""
            Container monitor
            Usage and description
              {heading}
                snyk container monitor [<OPTIONS>]
                  [<IMAGE>]

                The snyk container monitor command captures image dependencies
                and monitors the snapshot for vulnerabilities.

            Options
              --json
                Emit JSON.
            """;

        var command = (await new TestSnykCliScraper().Parse(["snyk", "container", "monitor"], help))!;
        await Assert.That(command.Description).IsEqualTo("The snyk container monitor command captures image dependencies and monitors the snapshot for vulnerabilities.");
    }

    [Test]
    [Arguments("")]
    [Arguments(":")]
    public async Task Explicit_Description_Takes_Precedence_Over_Prerequisites(string headingSuffix)
    {
        var help = $"""
            SBOM
            Prerequisites
              Feature availability: This feature is available only to customers on Snyk Enterprise plans.

            Usage
              $ snyk sbom --format=<cyclonedx1.4+json|spdx2.3+json> [OPTIONS]

            Description{headingSuffix}
              The snyk sbom command generates an SBOM for a local software project in an ecosystem supported by
              Snyk.

            Options
              --format=<cyclonedx1.4+json|spdx2.3+json>
                Required. Specify the output format.
            """;

        var command = (await new TestSnykCliScraper().Parse(["snyk", "sbom"], help))!;
        await Assert.That(command.Description).IsEqualTo("The snyk sbom command generates an SBOM for a local software project in an ecosystem supported by Snyk.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--format").IsRequired).IsTrue();
    }

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
    public async Task Command_Help_Does_Not_Treat_Examples_As_A_Command_Group()
    {
        const string helpText = """
            Test
            Usage
              snyk test [<OPTIONS>]

            Options for build tools
              The format is snyk <command> -- [<context-specific_options>]

            Examples for the snyk test command
              $ snyk test
            """;

        var scraper = new TestSnykCliScraper();
        using (Assert.Multiple())
        {
            await Assert.That(scraper.Extract(helpText)).IsEmpty();
            await Assert.That(scraper.DeclaresCommandGroup(helpText)).IsFalse();
        }
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
              --filter
                  Specify an OPA filter expression.
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
                     "--filter",
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
    public async Task Option_Parser_Ignores_Indented_Example_Fragments()
    {
        const string helpText = """
            Test infrastructure.

            Options
              --target-reference=<TARGET_REFERENCE>
                  Set a target reference, for example from this command:
                    --abbrev=0

                --platform=<PLATFORM>
                  Select a container platform.
              --json
                  Print JSON output.
            """;

        var command = await new TestSnykCliScraper().Parse(["snyk", "iac", "test"], helpText);

        await Assert.That(command!.Options.Select(x => x.SwitchName))
            .IsEquivalentTo(["--target-reference", "--platform", "--json"]);
    }

    [Test]
    public async Task Option_Parser_Ignores_Switches_In_Heading_Prose()
    {
        const string helpText = """
            Test a project.

            Options
              --prune-repeated-subdependencies, -p. See the --prune-repeated subdependencies option help
                Prune dependency trees.
            """;

        var command = await new TestSnykCliScraper().Parse(["snyk", "test"], helpText);

        await Assert.That(command!.Options.Select(x => x.SwitchName))
            .IsEquivalentTo(["--prune-repeated-subdependencies"]);
    }

    [Test]
    public async Task Iac_Describe_From_Glob_Remains_Scalar()
    {
        const string helpText = """
            Describe infrastructure as code.

            Options
              --from=<PATH>
                Specify multiple Terraform state files to be read. Glob patterns are supported.
            """;

        var scraper = new TestSnykCliScraper();
        var command = await scraper.Parse(["snyk", "iac", "describe"], helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.SwitchName).IsEqualTo("--from");
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
            await Assert.That(scraper.IsKnownScalar(["iac", "describe"], "--from"))
                .IsTrue();
        }
    }

    [Test]
    public async Task Root_Test_Models_Optional_Target()
    {
        var command = await new TestSnykCliScraper().Parse(
            ["snyk", "test"],
            "Usage: snyk test [<TARGET>] [<OPTIONS>]");

        var target = command!.PositionalArguments.Single();
        await Assert.That(target.PropertyName).IsEqualTo("Target");
        await Assert.That(target.IsRequired).IsFalse();
    }

    [Test]
    public async Task Monitor_Models_Optional_Target()
    {
        var command = await new TestSnykCliScraper().Parse(
            ["snyk", "monitor"],
            "Usage: snyk monitor [<TARGET>] [<OPTIONS>]");

        var target = command!.PositionalArguments.Single();
        await Assert.That(target.PropertyName).IsEqualTo("Target");
        await Assert.That(target.IsRequired).IsFalse();
    }

    [Test]
    public async Task Commands_With_Different_Value_Sets_Get_Distinct_Enums()
    {
        const string rootHelp = """
            Options
              --fail-on=<all|upgradable|patchable>
                  Fail only for fixable vulnerabilities.
            """;
        const string containerHelp = """
            Options
              --fail-on=<all|upgradable>
                  Fail only for fixable vulnerabilities.
            """;
        var scraper = new TestSnykCliScraper();

        var root = await scraper.Parse(["snyk", "test"], rootHelp);
        var container = await scraper.Parse(["snyk", "container", "test"], containerHelp);
        var commands = SnykCliScraper.DisambiguateEnumNames([root!, container!]);
        var rootEnum = commands[0].Options.Single().EnumDefinition!;
        var containerEnum = commands[1].Options.Single().EnumDefinition!;

        await Assert.That(rootEnum.EnumName).IsEqualTo("SnykFailOn");
        await Assert.That(containerEnum.EnumName).IsEqualTo("SnykContainerTestFailOn");
        await Assert.That(containerEnum.Values.Select(x => x.CliValue)).DoesNotContain("patchable");
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
    public async Task Iac_Group_Does_Not_Model_Child_Path_On_Parent()
    {
        var command = await new TestSnykCliScraper().Parse(
            ["snyk", "iac"],
            "Usage: snyk iac <COMMAND> [<OPTIONS>] [<PATH>]");

        await Assert.That(command!.PositionalArguments).IsEmpty();
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

        public bool DeclaresCommandGroup(string helpText) => HelpDeclaresCommandGroup(helpText);

        public bool CanGenerate(string helpText) => HasOptions(helpText);

        public bool IsKnownScalar(IReadOnlyList<string> commandParts, string switchName) =>
            ShouldTreatOptionAsScalar(commandParts, switchName);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
            => ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }
}
