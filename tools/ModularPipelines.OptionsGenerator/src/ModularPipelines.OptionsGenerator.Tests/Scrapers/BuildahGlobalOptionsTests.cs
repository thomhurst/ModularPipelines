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
    public async Task Full_Traversal_Preserves_Commands_And_Local_Mapping_Overrides()
    {
        var scraper = new TestScraper();
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        await Assert.That(commands).Count().IsEqualTo(37);
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
            CancellationToken cancellationToken = default, string? workingDirectory = null)
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
