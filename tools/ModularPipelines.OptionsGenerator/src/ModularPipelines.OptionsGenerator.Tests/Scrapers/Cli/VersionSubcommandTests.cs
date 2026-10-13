using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class VersionSubcommandTests
{
    [Test]
    [Arguments("helm", "--help", "--short", "bool?")]
    [Arguments("kubectl", "--help", "--client", "bool?")]
    [Arguments("terraform", "-help", "-json", "bool?")]
    public async Task Version_Subcommands_Reach_Generated_Options_And_Services(
        string toolName, string helpSwitch, string optionSwitch, string optionType)
    {
        var rootHelp = toolName == "terraform"
            ? """
                Usage: terraform [global options] <subcommand> [args]

                All other commands:
                  version       Show the current Terraform version

                Global options (use these before the subcommand, if any):
                  -chdir=DIR    Switch working directory.
                  -version      An alias for the "version" subcommand.
                """
            : $"""
                Usage:
                  {toolName} [command]

                Available Commands:
                  version       Print version information
                """;
        var versionHelp = toolName == "terraform"
            ? """
                Usage: terraform [global options] version [options]

                  Displays the version of Terraform and all installed plugins

                Options:
                  -json       Output the version information as a JSON object.
                """
            : $"""
                Print version information.

                Usage:
                  {toolName} version [flags]

                Flags:
                  {optionSwitch}       Print client version information
                """;
        var executor = new HelpExecutor(new Dictionary<string, string>
        {
            [helpSwitch] = rootHelp,
            [$"version {helpSwitch}"] = versionHelp,
        });
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        ICliScraper scraper = toolName switch
        {
            "helm" => new HelmCliScraper(executor, cache, NullLogger<HelmCliScraper>.Instance),
            "kubectl" => new KubectlCliScraper(executor, cache, NullLogger<KubectlCliScraper>.Instance),
            "terraform" => new TerraformCliScraper(executor, cache, NullLogger<TerraformCliScraper>.Instance),
            _ => throw new ArgumentOutOfRangeException(nameof(toolName)),
        };
        var commands = await Scrape(scraper);
        var command = commands.Single();
        var definition = (await scraper.CreateToolDefinitionAsync()) with { Commands = commands };
        var optionFiles = await new OptionsClassGenerator().GenerateAsync(definition);
        var serviceFiles = await new ServiceInterfaceGenerator().GenerateAsync(definition);

        await Assert.That(command.FullCommand).IsEqualTo($"{toolName} version");
        await Assert.That(command.Options.Single(option => option.SwitchName == optionSwitch).CSharpType)
            .IsEqualTo(optionType);
        await Assert.That(definition.GlobalOptions.Select(option => option.SwitchName)).DoesNotContain("-version");
        await Assert.That(optionFiles.Single().Content).Contains("CliSubCommand(\"version\")");
        await Assert.That(serviceFiles.Select(file => file.Content))
            .Contains(content => content.Contains($"VersionAsync({command.ClassName}"));
        await Assert.That(executor.Arguments).IsEquivalentTo([helpSwitch, $"version {helpSwitch}"]);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Global_Version_Flag_Is_Excluded_Only_When_Subcommand_Exists(bool hasVersionCommand)
    {
        var executor = new HelpExecutor(new Dictionary<string, string>
        {
            ["--help"] = $"""
                Usage: fake [command]

                Available Commands:
                  {(hasVersionCommand ? "version" : "run")}    Execute command

                Global Flags:
                  -V, --version     Print tool version
                  --verbose         Enable verbose output
                """,
            [$"{(hasVersionCommand ? "version" : "run")} --help"] = $"""
                Print version information.

                Usage: fake {(hasVersionCommand ? "version" : "run")} [flags]

                Flags:
                  --version string  Select resource version
                """,
        });
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new VersionScraper(executor, cache);
        var command = (await Scrape(scraper)).Single();
        var definition = await scraper.CreateToolDefinitionAsync();

        await Assert.That(definition.GlobalOptions.Any(option => option.SwitchName == "--version"))
            .IsEqualTo(!hasVersionCommand);
        await Assert.That(definition.SupplementalGlobalOptions.Any(option => option.SwitchName == "--version"))
            .IsEqualTo(!hasVersionCommand);
        await Assert.That(definition.GlobalOptions.Select(option => option.SwitchName)).Contains("--verbose");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--version").CSharpType)
            .IsEqualTo("string?");
    }

    [Test]
    public async Task Version_Command_Without_Flags_Is_Retained_While_Utility_Commands_Are_Skipped()
    {
        var executor = new HelpExecutor(new Dictionary<string, string>
        {
            ["--help"] = """
                Usage: fake [command]

                Available Commands:
                  version           Print version information
                  help              Show help
                  completion        Generate completions
                  __complete        Complete command
                  __completeNoDesc  Complete command without descriptions
                  --version         Print version information
                """,
            ["version --help"] = """
                Print version information.

                Usage: fake version
                """,
        });
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var command = (await Scrape(new VersionScraper(executor, cache))).Single();

        await Assert.That(command.FullCommand).IsEqualTo("fake version");
        await Assert.That(command.Options).IsEmpty();
        await Assert.That(command.PositionalArguments).IsEmpty();
        await Assert.That(executor.Arguments).IsEquivalentTo(["--help", "version --help"]);
    }

    [Test]
    public async Task Azure_Version_Without_Local_Options_Reaches_Generated_Options_And_Services()
    {
        var globals = """

            Global Arguments
                --debug : Increase logging verbosity.
                --output -o : Output format. Allowed values: json, table, tsv.
            """ + "\n" + AzCliMetadataExecutor.MetadataMarker + """
            {"--debug":{"IsInteger":false,"IsRepeated":false,"Nargs":"0"},"--output":{"IsInteger":false,"IsRepeated":false,"Nargs":"1"}}
            """;
        var executor = new HelpExecutor(new Dictionary<string, string>
        {
            ["--help"] = """
                Group
                    az : Manage Azure resources.

                Commands:
                    version : Show Azure CLI version information.
                """ + "\n" + globals,
            ["version --help"] = """
                Command
                    az version : Show Azure CLI version information.

                """ + "\n" + globals,
        });
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new AzCliScraper(executor, cache, NullLogger<AzCliScraper>.Instance);
        var commands = await Scrape(scraper);
        var command = commands.Single();
        var definition = scraper.CreateToolDefinition() with { Commands = commands };
        var optionFiles = await new OptionsClassGenerator().GenerateAsync(definition);
        var serviceFiles = await new ServiceInterfaceGenerator().GenerateAsync(definition);

        await Assert.That(command.FullCommand).IsEqualTo("az version");
        await Assert.That(command.Options).IsEmpty();
        await Assert.That(definition.GlobalOptions.Single(option => option.SwitchName == "--debug").IsFlag).IsTrue();
        await Assert.That(definition.GlobalOptions.Single(option => option.SwitchName == "--output").ShortForm).IsEqualTo("-o");
        await Assert.That(optionFiles.Single().Content).Contains("CliSubCommand(\"version\")");
        await Assert.That(serviceFiles.Select(file => file.Content))
            .Contains(content => content.Contains($"VersionAsync({command.ClassName}"));
        await Assert.That(executor.Arguments).IsEquivalentTo(["--help", "version --help"]);
    }

    [Test]
    [Arguments("empty-help")]
    [Arguments("failed-help")]
    [Arguments("wrong-help")]
    [Arguments("parse-exception")]
    [Arguments("null-definition")]
    public async Task Discovered_Version_Without_Valid_Definition_Fails_Scrape(string failure)
    {
        var executor = new HelpExecutor(new Dictionary<string, string>
        {
            ["--help"] = """
                Usage: fake [command]

                Available Commands:
                  version           Print version information

                Global Flags:
                  -V, --version     Print tool version
                """,
            ["version --help"] = failure switch
            {
                "empty-help" => string.Empty,
                "wrong-help" => "Usage: fake run [flags]",
                _ => "Usage: fake version",
            },
        }, failVersionHelp: failure == "failed-help");
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FailingVersionScraper(executor, cache, failure);

        await Assert.That(async () => { await Scrape(scraper); })
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("fake version was discovered, but no valid command definition was produced");
    }

    private static async Task<IReadOnlyList<CliCommandDefinition>> Scrape(ICliScraper scraper)
    {
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return commands;
    }

    private class VersionScraper(ICliCommandExecutor executor, IHelpTextCache cache)
        : CobraCliScraper(executor, cache, NullLogger<VersionScraper>.Instance)
    {
        public override string ToolName => "fake";
        public override string NamespacePrefix => "Fake";
        public override string TargetNamespace => "ModularPipelines.Fake";
        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
            ParseNamedOptionSection(helpText, "Global Flags", []);

        protected override IReadOnlyList<CliOptionDefinition> SupplementalGlobalOptions =>
        [new() { SwitchName = "--version", ShortForm = "-V", PropertyName = "Version", CSharpType = "bool?", IsFlag = true }];
    }

    private sealed class FailingVersionScraper(ICliCommandExecutor executor, IHelpTextCache cache, string failure)
        : VersionScraper(executor, cache)
    {
        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath, string helpText, UsageSynopsisParseResult usage,
            CancellationToken cancellationToken) => failure switch
            {
                "parse-exception" => throw new InvalidOperationException("Invalid version help"),
                "null-definition" => Task.FromResult<CliCommandDefinition?>(null),
                _ => base.ParseCommandAsync(commandPath, helpText, usage, cancellationToken),
            };
    }

    private sealed class HelpExecutor(IReadOnlyDictionary<string, string> help, bool failVersionHelp = false)
        : ICliCommandExecutor
    {
        public List<string> Arguments { get; } = [];

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(
            string command, string arguments, CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Arguments.Add(arguments);
            return Task.FromResult(new CliCommandResult
            {
                ExitCode = failVersionHelp && arguments == "version --help" ? 1 : 0,
                ExecutionFailed = failVersionHelp && arguments == "version --help",
                StandardOutput = help[arguments],
                StandardError = string.Empty,
            });
        }
    }
}
