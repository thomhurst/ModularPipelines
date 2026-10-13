using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class EksctlGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches = ["--color", "--dumpLogs", "--verbose"];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Root_Common_Flags_Are_Persistent_With_Original_Shapes(string newline)
    {
        var globals = new TestScraper().Globals(Fixture("root").ReplaceLineEndings(newline));
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        var color = globals.Single(option => option.SwitchName == "--color");
        await Assert.That(color.IsFlag).IsFalse();
        await Assert.That(color.ShortForm).IsEqualTo("-C");
        var verbose = globals.Single(option => option.SwitchName == "--verbose");
        await Assert.That(verbose.CSharpType).IsEqualTo("int?");
        await Assert.That(verbose.ShortForm).IsEqualTo("-v");
        var dump = globals.Single(option => option.SwitchName == "--dumpLogs");
        await Assert.That(dump.PropertyName).IsEqualTo("Dumplogs");
        await Assert.That(dump.IsFlag).IsTrue();
        await Assert.That(dump.ShortForm).IsEqualTo("-d");
        foreach (var option in globals)
        {
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
        }
    }

    [Test]
    [Arguments("create")]
    [Arguments("create-cluster")]
    [Arguments("get-cluster")]
    [Arguments("version")]
    public async Task Captured_Help_Preserves_Inherited_Shapes(string fixture)
    {
        var scraper = new TestScraper();
        var globals = scraper.Globals(Fixture("root"));
        var command = (await scraper.Parse(["eksctl", .. fixture.Split('-')], Fixture(fixture)))!;
        var inherited = command.Options.Where(option => GlobalSwitches.Contains(option.SwitchName)).ToList();
        await Assert.That(inherited.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        await Assert.That(CliGlobalOptionMerger.Merge(globals, inherited)).Count().IsEqualTo(3);
    }

    [Test]
    public async Task Traversal_Deduplicates_Globals_And_Keeps_Aws_Settings_Local()
    {
        var scraper = new TestScraper(new HelpExecutor());
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        var cluster = commands.Single(command => command.CommandParts.SequenceEqual(["get", "cluster"]));
        await Assert.That(cluster.Options.Any(option => GlobalSwitches.Contains(option.SwitchName))).IsFalse();
        await Assert.That(cluster.Options.Any(option => option.SwitchName == "--region")).IsTrue();
        await Assert.That(cluster.Options.Any(option => option.SwitchName == "--profile")).IsTrue();
    }

    [Test]
    public async Task Conflicting_Inherited_Arity_Is_Rejected()
    {
        var scraper = new TestScraper(new HelpExecutor());
        await scraper.ScrapeAsync().ToListAsync();
        var help = Fixture("get-cluster").Replace("--verbose int", "--verbose string", StringComparison.Ordinal);
        await Assert.That(async () => await scraper.Parse(["eksctl", "get", "cluster"], help))
            .Throws<InvalidOperationException>();
    }

    private static string Fixture(string name) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Eksctl", "0.231.0", name + ".txt"));

    private sealed class TestScraper(ICliCommandExecutor? executor = null) : EksctlCliScraper(
        executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<EksctlCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);

        public Task<CliCommandDefinition?> Parse(string[] path, string help) =>
            ParseCommandAsync(path, help, ParseUsageSynopsis(path, help), CancellationToken.None);
    }

    private sealed class HelpExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var root = Fixture("root");
            var common = root[root.IndexOf("Common flags:", StringComparison.Ordinal)..];
            var help = arguments switch
            {
                "--help" => "Usage: eksctl [command] [flags]\n\nCommands:\n  eksctl get   Get resources\n\n" + common,
                "get --help" => "Usage: eksctl get [flags]\n\nCommands:\n  eksctl get cluster   Get clusters\n\n" + common,
                "get cluster --help" => Fixture("get-cluster"),
                "version" => "0.231.0",
                _ => throw new InvalidOperationException($"Unexpected arguments: {arguments}"),
            };
            return Task.FromResult(new CliCommandResult { StandardOutput = help, StandardError = string.Empty, ExitCode = 0 });
        }
    }
}
