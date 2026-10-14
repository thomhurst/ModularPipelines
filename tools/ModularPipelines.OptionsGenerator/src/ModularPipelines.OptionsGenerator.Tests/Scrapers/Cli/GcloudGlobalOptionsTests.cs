using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudGlobalOptionsTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Wide_Flags_Are_Inherited_Without_Losing_Local_Overrides(bool windowsNewlines)
    {
        const string rootHelp = """
            SYNOPSIS
                gcloud COMMAND [GCLOUD_WIDE_FLAG ...]
            GLOBAL FLAGS
                 --project=PROJECT_ID
                    The project ID.
                 --quiet, -q
                    Disable prompts.
                 --flatten=[KEY,...]
                    Flatten output.
                 --flags-file=YAML_FILE
                    This flag may be repeated.
            OTHER FLAGS
                 --version, -v
                    Print version information and exit. This flag is only available at the
                    global level.
                 --root-status
                    This flag is only available at the global level.
                 --[no-]user-output-enabled
                    Print output.
                 --no-user-output-enabled
                    Disable output.
                 --trace-token=TRACE_TOKEN
                    Token used to route traces.
            COMMANDS
                 list
                    List resources.
                 create
                    Create a resource.
            """;
        const string listHelp = """
            SYNOPSIS
                gcloud list [GCLOUD_WIDE_FLAG ...]
            FLAGS
                 --filter=FILTER
                    Filter resources.
            GCLOUD WIDE FLAGS
                These flags are available to all commands: --project, --quiet.
                Run $ gcloud help for details.
            """;
        const string createHelp = """
            SYNOPSIS
                gcloud create --project=PROJECT [GCLOUD_WIDE_FLAG ...]
            REQUIRED FLAGS
                 --project=PROJECT
                    The command-specific project selector.
            """;
        string Normalize(string text) => text.ReplaceLineEndings(windowsNewlines ? "\r\n" : "\n");
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new GcloudCliScraper(new FixtureExecutor(new Dictionary<string, string>
        {
            ["--help"] = Normalize(rootHelp),
            ["list --help"] = Normalize(listHelp),
            ["create --help"] = Normalize(createHelp),
        }), cache, NullLogger<GcloudCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var globals = tool.GetGlobalOptions();
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        await Assert.That(globals.Count).IsEqualTo(6);
        await Assert.That(globals.Single(option => option.SwitchName == "--project").PropertyType).IsEqualTo("string?");
        await Assert.That(globals.Single(option => option.SwitchName == "--project").ValueSeparator).IsEqualTo("=");
        await Assert.That(globals.Single(option => option.SwitchName == "--quiet").GetSwitchNames()).IsEquivalentTo(["--quiet", "-q"]);
        await Assert.That(globals.Single(option => option.SwitchName == "--quiet").ShortForm).IsEqualTo("-q");
        await Assert.That(globals.Single(option => option.SwitchName == "--flatten").CollectionSeparator).IsEqualTo(",");
        await Assert.That(globals.Single(option => option.SwitchName == "--flags-file").AcceptsMultipleValues).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--flags-file").CollectionSeparator).IsNull();
        await Assert.That(globals.Single(option => option.SwitchName == "--trace-token").IsSecret).IsTrue();
        var output = globals.Single(option => option.SwitchName == "--user-output-enabled");
        await Assert.That(output.NegatedSwitchName).IsEqualTo("--no-user-output-enabled");
        await Assert.That(output.PropertyType).IsEqualTo("bool?");
        var list = tool.Commands.Single(command => command.FullCommand == "gcloud list");
        await Assert.That(list.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--filter"]);
        var create = tool.Commands.Single(command => command.FullCommand == "gcloud create");
        await Assert.That(create.Options.Single().IsRequired).IsTrue();
        await Assert.That(create.Options.Single().Description).Contains("command-specific");
        var baseClass = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(baseClass).Contains("public virtual string? Project { get; set; }");
        await Assert.That(baseClass).DoesNotContain("[CliGlobalOptions]");
        await Assert.That(baseClass).Contains("[CliFlag(\"--quiet\", ShortForm = \"-q\")]");
        await Assert.That(baseClass).DoesNotContain("--version");
        await Assert.That(baseClass).DoesNotContain("--root-status");
        var classes = await new OptionsClassGenerator().GenerateAsync(tool);
        await Assert.That(classes.Single(file => file.RelativePath.Contains("GcloudListOptions")).Content)
            .DoesNotContain("public override string? Project");
        await Assert.That(classes.Single(file => file.RelativePath.Contains("GcloudCreateOptions")).Content)
            .Contains("public new string Project { get; private init; }");
    }

    [Test]
    public async Task Captured_Root_Help_Provides_Project_And_Negatable_Output()
    {
        var rootHelp = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "589.0.0", "gcloud-help.txt"));
        // Restrict discovery while retaining the installed help's complete flag definitions.
        rootHelp = rootHelp[..rootHelp.IndexOf("\nGROUPS", StringComparison.Ordinal)];
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new GcloudCliScraper(new FixtureExecutor(new Dictionary<string, string>
        {
            ["--help"] = rootHelp,
        }), cache, NullLogger<GcloudCliScraper>.Instance);
        await foreach (var _ in scraper.ScrapeAsync())
        {
        }

        var globals = scraper.CreateToolDefinition().GetGlobalOptions();
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(
        [
            "--account", "--billing-project", "--configuration", "--flags-file", "--flatten", "--format",
            "--project", "--quiet", "--verbosity", "--access-token-file", "--impersonate-service-account",
            "--log-http", "--trace-token", "--user-output-enabled",
        ]);
        await Assert.That(globals.Single(option => option.SwitchName == "--project").PropertyType).IsEqualTo("string?");
        await Assert.That(globals.Single(option => option.SwitchName == "--user-output-enabled").NegatedSwitchName)
            .IsEqualTo("--no-user-output-enabled");
        await Assert.That(globals.Single(option => option.SwitchName == "--impersonate-service-account").CollectionSeparator)
            .IsEqualTo(",");
        await Assert.That(globals.Single(option => option.SwitchName == "--quiet").GetSwitchNames()).IsEquivalentTo(["--quiet", "-q"]);
        await Assert.That(globals.Single(option => option.SwitchName == "--flags-file").AcceptsMultipleValues).IsTrue();
    }

    private sealed class FixtureExecutor(IReadOnlyDictionary<string, string> help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = help.GetValueOrDefault(arguments, string.Empty),
                StandardError = string.Empty,
                ExitCode = help.ContainsKey(arguments) ? 0 : 1,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
