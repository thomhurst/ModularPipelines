using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class BuildahGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--cgroup-manager", "--log-level", "--registries-conf", "--registries-conf-dir", "--root",
        "--runroot", "--short-name-alias-conf", "--storage-driver", "--storage-opt", "--userns-gid-map", "--userns-uid-map",
    ];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Public_Persistent_Settings_Exclude_Hidden_Flags_And_Root_Controls(string newline)
    {
        var options = new TestScraper().Globals(Fixture("root-help").ReplaceLineEndings(newline));
        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        foreach (var name in new[] { "--storage-opt", "--userns-gid-map", "--userns-uid-map" })
        {
            var option = options.Single(option => option.SwitchName == name);
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.GroupValues).IsFalse();
            await Assert.That(option.CollectionSeparator).IsNull();
        }
        await Assert.That(options.Any(option => option.IsSecret)).IsFalse();
    }

    [Test]
    public async Task Config_Healthcheck_Retries_Number_Placeholder_Remains_Numeric()
    {
        var commands = await new TestScraper().ScrapeAsync().ToListAsync();
        var option = commands.Single(command => command.FullCommand == "buildah config").Options
            .Single(option => option.SwitchName == "--healthcheck-retries");

        await Assert.That(option.CSharpType).IsEqualTo("int?");
        await Assert.That(option.IsNumeric).IsTrue();
        await Assert.That(option.AcceptsMultipleValues).IsFalse();
    }

    [Test]
    public async Task Full_Traversal_Preserves_Commands_And_Local_Mapping_Overrides()
    {
        var scraper = new TestScraper();
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        await Assert.That(commands).Count().IsEqualTo(38);
        await Assert.That(commands.Single(command => command.FullCommand == "buildah version").Options
            .Single(option => option.SwitchName == "--json").IsFlag).IsTrue();
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        foreach (var verb in new[] { "build", "from" })
        {
            var command = commands.Single(command => command.FullCommand == "buildah " + verb);
            foreach (var name in new[] { "--userns-gid-map", "--userns-uid-map" })
            {
                var local = command.Options.Single(option => option.SwitchName == name);
                await Assert.That(local.CSharpType).IsEqualTo("IEnumerable<string>?");
                await Assert.That(local.Description).DoesNotContain("default ");
            }
        }
        var resolved = InheritedPropertyCollisionResolver.Resolve(tool);
        foreach (var verb in new[] { "build", "from" })
        {
            var mappings = resolved.Commands.Single(command => command.FullCommand == "buildah " + verb)
                .Options.Where(option => option.SwitchName is "--userns-gid-map" or "--userns-uid-map").ToArray();
            await Assert.That(mappings).Count().IsEqualTo(2);
            await Assert.That(mappings.All(option => option.ShadowsGlobalOption)).IsTrue();
        }
        await Assert.That(commands.Single(command => command.FullCommand == "buildah images").Options
            .Any(option => GlobalSwitches.Contains(option.SwitchName))).IsFalse();
    }

    [Test]
    [Arguments("string", false)]
    [Arguments("int", true)]
    public async Task Explicit_Inherited_Rows_Are_Validated_Before_Removal(string hint, bool conflict)
    {
        var scraper = new TestScraper();
        await scraper.ScrapeAsync().ToListAsync();
        var help = $"Usage: buildah images [flags]\n\nGlobal Flags:\n  --root {hint}    storage root dir\n";
        if (conflict)
        {
            await Assert.That(async () => await scraper.Parse(help)).Throws<InvalidOperationException>();
        }
        else
        {
            var command = await scraper.Parse(help);
            await Assert.That(command!.Options).IsEmpty();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Explicit_Inherited_Enums_Use_Root_Scope_And_Validate_Values(bool conflict)
    {
        var scraper = new TestScraper();
        await scraper.ScrapeAsync().ToListAsync();
        var logLevelRow = Fixture("root-help").Split('\n')
            .Single(line => line.Contains("--log-level", StringComparison.Ordinal));
        if (conflict)
        {
            logLevelRow = logLevelRow.Replace("\"trace\"", "\"verbose\"", StringComparison.Ordinal);
        }

        var help = $"Usage: buildah images [flags]\n\nGlobal Flags:\n{logLevelRow}\n";
        if (conflict)
        {
            await Assert.That(async () => await scraper.Parse(help)).Throws<InvalidOperationException>();
        }
        else
        {
            var command = await scraper.Parse(help);
            await Assert.That(command!.Options).IsEmpty();
            await Assert.That(command.Enums).IsEmpty();
            await Assert.That(scraper.CreateToolDefinition().GlobalOptions.Single(option => option.SwitchName == "--log-level")
                .EnumDefinition!.EnumName).IsEqualTo("BuildahLogLevel");
            var enums = await new EnumGenerator().GenerateAsync(scraper.CreateToolDefinition() with { Commands = [command] });
            await Assert.That(enums.Select(file => Path.GetFileName(file.RelativePath)))
                .IsEquivalentTo(["BuildahLogLevel.Generated.cs"]);
        }
    }

    [Test]
    public async Task Explicit_Inherited_Enums_Preserve_Local_And_Group_Enums()
    {
        var scraper = new TestScraper();
        await scraper.ScrapeAsync().ToListAsync();
        var logLevelRow = Fixture("root-help").Split('\n')
            .Single(line => line.Contains("--log-level", StringComparison.Ordinal));
        var command = await scraper.Parse($"""
            Usage: buildah images [flags]

            Flags:
              --log-level string    Local log level, one of "debug", "info"

            Global Flags:
            {logLevelRow}
              --group-level string    Group log level, one of "debug", "info"
            """);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--log-level", "--group-level"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--log-level").Description)
            .StartsWith("Local log level");
        await Assert.That(command.Enums).Count().IsEqualTo(2);
        await Assert.That(command.Enums.Any(definition => definition.EnumName == "BuildahLogLevel")).IsFalse();
        await Assert.That(scraper.CreateToolDefinition().GlobalOptions.Any(option => option.SwitchName == "--group-level"))
            .IsFalse();
    }

    [Test]
    public async Task Explicit_Inherited_Rows_Do_Not_Remove_Local_Overrides_Or_Group_Settings()
    {
        var scraper = new TestScraper();
        await scraper.ScrapeAsync().ToListAsync();
        var command = await scraper.Parse("""
            Usage: buildah images [flags]

            Flags:
              --userns-uid-map ctrID:hostID:length    Local mapping

            Global Flags:
              --userns-uid-map ctrID:hostID:length    Default mapping
              --root string                         Storage root
              --group-setting string                Group-local setting
            """);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--userns-uid-map", "--group-setting"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--userns-uid-map").Description)
            .IsEqualTo("Local mapping");
        await Assert.That(scraper.CreateToolDefinition().GlobalOptions.Any(option => option.SwitchName == "--group-setting"))
            .IsFalse();
    }

    private static string Fixture(string name) => File.ReadAllText(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", "Buildah", "1.33.7", name + ".txt"));

    private sealed class TestScraper : BuildahCliScraper
    {
        public TestScraper() : base(new HelpExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<BuildahCliScraper>.Instance)
        {
        }

        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);

        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(["buildah", "images"], help,
            ParseUsageSynopsis(["buildah", "images"], help), CancellationToken.None);
    }

    private sealed class HelpExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            string? workingDirectory = null, CancellationToken cancellationToken = default)
        {
            var fixture = arguments switch
            {
                "--help" => "root-help",
                "--version" => "version",
                _ when arguments.EndsWith(" --help", StringComparison.Ordinal) => arguments[..^7].Replace(' ', '-') + "-help",
                _ => throw new InvalidOperationException($"Unexpected help command: {arguments}"),
            };
            return Task.FromResult(new CliCommandResult { StandardOutput = Fixture(fixture), StandardError = string.Empty, ExitCode = 0 });
        }
    }
}
