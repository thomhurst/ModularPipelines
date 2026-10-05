using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AzArgumentShapeTests
{
    [Test]
    [Arguments("+", false)]
    [Arguments("*", true)]
    [Arguments("2", false)]
    public async Task Repeated_Groups_Preserve_Occurrence_Boundaries(string nargs, bool optional)
    {
        var option = await Parse("identities", "Identity values.", nargs, false, true);
        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<CliValueGroup>?");
        await Assert.That(option.GroupValues).IsTrue();
        await Assert.That(option.ValueArity).IsEqualTo(optional ? CliOptionValueArity.Optional : CliOptionValueArity.Required);
    }

    [Test]
    public async Task Json_File_Array_Prose_Does_Not_Reject_Verified_Scalar_During_Traversal()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "AzCli", "az-2.84.0-batch-task-create.txt"));
        var scraper = new AzCliScraper(new BatchHelpExecutor(help),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<AzCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var option = commands.Single(command => command.FullCommand == "az batch task create")
            .Options.Single(option => option.SwitchName == "--json-file");
        await Assert.That(option.PropertyType).IsEqualTo("string?");
        await Assert.That(option.AcceptsMultipleValues).IsFalse();
    }

    private sealed class BatchHelpExecutor(string help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => "Group\n    az : Azure.\nSubgroups:\n    batch : Batch.",
                    "batch --help" => "Group\n    az batch : Batch.\nSubgroups:\n    task : Tasks.",
                    "batch task --help" => "Group\n    az batch task : Tasks.\nCommands:\n    create : Create tasks.",
                    "batch task create --help" => help,
                    _ => throw new InvalidOperationException(arguments),
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    [Test]
    [Arguments("aks-create", "--node-vm-size", "string?", false)]
    [Arguments("aks-nodepool-add", "--node-vm-size", "string?", false)]
    [Arguments("aks-nodepool-add", "--max-unavailable", "string?", false)]
    [Arguments("aks-nodepool-update", "--max-unavailable", "string?", false)]
    [Arguments("aks-nodepool-add", "--max-surge", "string?", false)]
    [Arguments("aks-nodepool-update", "--max-surge", "string?", false)]
    [Arguments("aro-create", "--master-vm-size", "string?", false)]
    [Arguments("aro-create", "--worker-vm-size", "string?", false)]
    [Arguments("aks-create", "--enable-azure-container-storage", "IEnumerable<CliOptionValue>?", true)]
    [Arguments("aks-update", "--enable-azure-container-storage", "IEnumerable<CliOptionValue>?", true)]
    [Arguments("pipelines-run", "--parameters", "IEnumerable<CliOptionValue>?", true)]
    [Arguments("pipelines-run", "--variables", "IEnumerable<CliOptionValue>?", true)]
    [Arguments("boards-work-item-create", "--fields", "IEnumerable<CliOptionValue>?", true)]
    [Arguments("boards-work-item-update", "--fields", "IEnumerable<CliOptionValue>?", true)]
    [Arguments("aks-create", "--node-count", "int?", false)]
    public async Task CapturedInstalledParserPreservesShape(string fixture, string name, string propertyType, bool grouped)
    {
        // Azure CLI 2.84.0, azure-devops 1.0.8. Help and metadata come from the same invocation.
        var option = await ParseFixture(fixture, name);
        await Assert.That(option.PropertyType).IsEqualTo(propertyType);
        await Assert.That(option.GroupValues).IsEqualTo(grouped);
    }

    internal static async Task<CliOptionDefinition> ParseFixture(string fixture, string name)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "AzCli", $"az-2.84.0-{fixture}.txt"));
        var command = await new Scraper().Parse(help);
        return command!.Options.Single(option => option.SwitchName == name);
    }

    [Test]
    [Arguments("storage blob upload", "--overwrite")]
    [Arguments("webapp create", "--assign-identity")]
    public async Task LegacyManualOverridesCannotReplaceInstalledParserContract(string commandName, string switchName)
    {
        var original = await ParseFixture(commandName.Replace(' ', '-'), switchName);
        var detector = new ManualOverrideDetector(NullLogger<ManualOverrideDetector>.Instance, Path.Combine(AppContext.BaseDirectory, "TypeOverrides"));
        var enhancer = new OptionTypeEnhancer(new OptionTypeDetectorPipeline([detector], NullLogger<OptionTypeDetectorPipeline>.Instance), NullLogger<OptionTypeEnhancer>.Instance);
        var tool = new Scraper().CreateToolDefinition() with
        {
            Commands = [new() { FullCommand = $"az {commandName}", CommandParts = commandName.Split(' '), ClassName = "AzTestOptions", ParentClassName = "AzOptions", ToolNamespacePrefix = "Az", Options = [original] }],
        };
        var enhanced = (await enhancer.EnhanceManualOverridesAsync(tool)).Commands.Single().Options.Single();
        await Assert.That(enhanced.CSharpType).IsEqualTo(original.CSharpType);
        await Assert.That(enhanced.IsFlag).IsEqualTo(original.IsFlag);
        await Assert.That(enhanced.GroupValues).IsEqualTo(original.GroupValues);
        await Assert.That(enhanced.ValueArity).IsEqualTo(original.ValueArity);
    }

    [Test]
    public async Task VersionIdentityIncludesSortedExtensionsWithoutHostDetails()
    {
        var version = new Scraper().Version("""{"azure-cli":"2.90.0","azure-cli-core":"2.90.0","extensions":{"z-extension":"2.0.0","azure-devops":"1.0.8"}}""");
        await Assert.That(version).IsEqualTo("azure-cli 2.90.0; azure-devops 1.0.8; z-extension 2.0.0");
    }

    [Test]
    public async Task LoginIsExcludedWithSupportedEnvironmentAuthenticationAlternative()
    {
        var scraper = new Scraper();
        var command = await scraper.Parse("Command\n    az devops login : Log in.\nArguments\n    --organization : Organization URL.", ["az", "devops", "login"]);
        await Assert.That(command).IsNull();
        var exclusion = scraper.CreateToolDefinition().CommandCoverage.Exclusions.Single(value => value.Command == "az devops login");
        await Assert.That(exclusion.Reason).Contains("CommandExecutionOptions.EnvironmentVariables");
    }

    [Test]
    [Arguments("artifacts")]
    [Arguments("boards")]
    [Arguments("devops")]
    [Arguments("pipelines")]
    [Arguments("repos")]
    public async Task DevOpsCommandsDocumentExtensionPrerequisite(string group)
    {
        var command = await new Scraper().Parse($"Command\n    az {group} run : Run.\nArguments\n    --name : Name.", ["az", group, "run"]);
        await Assert.That(command!.Description).Contains("az extension add --name azure-devops");
    }

    [Test]
    [Arguments("null")]
    [Arguments("{}")]
    [Arguments("{\"Nargs\":\"unknown\",\"IsInteger\":false,\"IsRepeated\":false}")]
    public async Task InvalidShapeFailsInsteadOfGuessing(string shape)
    {
        var help = "Command\n    az test run : Run.\nArguments\n    --value : Value.\n"
            + AzCliMetadataExecutor.MetadataMarker + "{\"--value\":" + shape + "}";
        await Assert.That(() => new Scraper().Parse(help)).ThrowsException();
    }

    [Test]
    [Arguments("node-vm-size", "Size of Virtual Machines to create as Kubernetes nodes.")]
    [Arguments("vm-size", "Size of Virtual Machines to create as Kubernetes nodes.")]
    [Arguments("master-vm-size", "Size of the master virtual machines.")]
    [Arguments("worker-vm-size", "Size of the worker virtual machines.")]
    [Arguments("max-unavailable", "The maximum number or percentage of nodes that can be unavailable.")]
    public async Task ScalarParserValuesOverrideNumericProse(string name, string description)
    {
        var option = await Parse(name, description, "1", false, false);
        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.IsNumeric).IsFalse();
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Required);
    }

    [Test]
    [Arguments("parameters")]
    [Arguments("variables")]
    [Arguments("fields")]
    public async Task ParserGroupsKeyValueInputs(string name)
    {
        var option = await Parse(name, "Values in name=value format.", "+", false, false);
        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.GroupValues).IsTrue();
    }

    [Test]
    [Arguments("*", "IEnumerable<CliOptionValue>?", true)]
    [Arguments("?", "CliOptionValue?", false)]
    public async Task OptionalParserValuesAllowBareSwitch(string nargs, string type, bool grouped)
    {
        var option = await Parse("enable-azure-container-storage", "Enable azure container storage.", nargs, false, false);
        await Assert.That(option.PropertyType).IsEqualTo(type);
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(option.GroupValues).IsEqualTo(grouped);
        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    public async Task IntegerParserValuesRemainNumeric()
    {
        var option = await Parse("node-count", "Kubernetes nodes.", "1", true, false);
        await Assert.That(option.CSharpType).IsEqualTo("int?");
        await Assert.That(option.IsNumeric).IsTrue();
    }

    [Test]
    public async Task AppendActionsRepeatSwitchInsteadOfGrouping()
    {
        var option = await Parse("value", "A value.", "1", false, true);
        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(option.GroupValues).IsFalse();
    }

    [Test]
    public async Task ScalarMetadataOverridesMisleadingListProse()
    {
        var option = await Parse("value", "A list of resources, encoded as JSON.", "1", false, false);
        await Assert.That(option.PropertyType).IsEqualTo("string?");
        await Assert.That(option.GroupValues).IsFalse();
    }

    private static async Task<CliOptionDefinition> Parse(string name, string description, string nargs, bool integer, bool repeated)
    {
        var metadata = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object>
        {
            [$"--{name}"] = new { Nargs = nargs, IsInteger = integer, IsRepeated = repeated },
        });
        var command = await new Scraper().Parse($"Command\n    az test run : Run.\nArguments\n    --{name} : {description}\n{AzCliMetadataExecutor.MetadataMarker}{metadata}");
        return command!.Options.Single();
    }

    [Test]
    [Arguments("{\"azure-cli\":null}")]
    [Arguments("{\"azure-cli\":\"\"}")]
    [Arguments("{\"azure-cli\":\"  \"}")]
    [Arguments("{\"azure-cli\":\"2.90.0\",\"extensions\":{\"azure-devops\":null}}")]
    [Arguments("{\"azure-cli\":\"2.90.0\",\"extensions\":{\"azure-devops\":\"\"}}")]
    public async Task Empty_Version_Values_Are_Rejected(string json)
    {
        await Assert.That(() => new Scraper().Version(json))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("reported an invalid version");
    }

    private sealed class Scraper() : AzCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<AzCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string help, string[]? path = null) => ParseCommandAsync(path ?? ["az", "test", "run"], help, CancellationToken.None);

        public string? Version(string json) => ParseVersionOutput(new CliCommandResult { ExitCode = 0, StandardOutput = json, StandardError = string.Empty });
    }
}
