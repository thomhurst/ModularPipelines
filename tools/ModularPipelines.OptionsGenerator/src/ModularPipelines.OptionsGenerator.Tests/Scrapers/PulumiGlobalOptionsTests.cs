using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PulumiGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--color", "--cwd", "--disable-integrity-checking", "--emoji", "--fully-qualify-stack-names",
        "--logflow", "--logtostderr", "--memprofilerate", "--non-interactive", "--otel-traces",
        "--profiling", "--tracing", "--verbose",
    ];

    [Test]
    [Arguments("\n", false)]
    [Arguments("\r\n", false)]
    [Arguments("\n", true)]
    public async Task Root_Help_Preserves_Persistent_Shapes_Across_Platforms(string newline, bool macDefault)
    {
        var help = Fixture("root").ReplaceLineEndings(newline);
        if (macDefault)
        {
            help = help.Replace("Enable emojis in the output", "Enable emojis in the output (default true)", StringComparison.Ordinal);
        }

        var options = new TestScraper().Globals(help);
        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        foreach (var option in options)
        {
            var expectedType = option.SwitchName switch
            {
                "--disable-integrity-checking" or "--emoji" or "--fully-qualify-stack-names"
                    or "--logflow" or "--logtostderr" or "--non-interactive" => "bool?",
                "--memprofilerate" or "--verbose" => "int?",
                _ => "string?",
            };
            var expectedFlag = expectedType == "bool?" && option.SwitchName != "--emoji";
            await Assert.That(option.CSharpType).IsEqualTo(expectedType);
            await Assert.That(option.IsFlag).IsEqualTo(expectedFlag);
            await Assert.That(option.ValueSeparator).IsEqualTo(expectedFlag ? " " : "=");
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
        }

        await Assert.That(options.Single(option => option.SwitchName == "--cwd").ShortForm).IsEqualTo("-C");
        await Assert.That(options.Single(option => option.SwitchName == "--emoji").ShortForm).IsEqualTo("-e");
        await Assert.That(options.Single(option => option.SwitchName == "--fully-qualify-stack-names").ShortForm).IsEqualTo("-Q");
        await Assert.That(options.Single(option => option.SwitchName == "--verbose").ShortForm).IsEqualTo("-v");
    }

    [Test]
    [Arguments("stack")]
    [Arguments("stack-ls")]
    [Arguments("up")]
    [Arguments("env")]
    [Arguments("env-run")]
    [Arguments("version")]
    public async Task Captured_Commands_Declare_Compatible_Inherited_Settings(string fixture)
    {
        var scraper = new TestScraper();
        var globals = scraper.Globals(Fixture("root"));
        var inherited = scraper.Inherited(Fixture(fixture));
        await Assert.That(inherited.Select(option => option.SwitchName).Intersect(GlobalSwitches))
            .IsEquivalentTo(GlobalSwitches);
        await Assert.That(CliGlobalOptionMerger.Merge(globals,
                inherited.Where(option => GlobalSwitches.Contains(option.SwitchName))))
            .Count().IsEqualTo(GlobalSwitches.Length);
    }

    [Test]
    public async Task Traversal_Deduplicates_Root_Settings_And_Preserves_Local_And_Group_Flags()
    {
        var scraper = new TestScraper();
        var commands = await scraper.ScrapeAsync().ToListAsync();
        await Assert.That(scraper.CreateToolDefinition().GlobalOptions.Select(option => option.SwitchName))
            .IsEquivalentTo(GlobalSwitches);
        await Assert.That(commands.SelectMany(command => command.Options)
            .Any(option => GlobalSwitches.Contains(option.SwitchName))).IsFalse();
        var list = commands.Single(command => command.FullCommand == "pulumi stack list");
        await Assert.That(list.Options.Select(option => option.SwitchName)).Contains("--project");
        await Assert.That(list.Options.Select(option => option.SwitchName)).DoesNotContain("--env");
        var up = commands.Single(command => command.FullCommand == "pulumi up");
        await Assert.That(up.Options.Select(option => option.SwitchName)).Contains("--stack");
        await Assert.That(up.Options.Select(option => option.SwitchName)).Contains("--config");
        var run = commands.Single(command => command.FullCommand == "pulumi env run");
        await Assert.That(run.Options.Select(option => option.SwitchName)).Contains("--env");
        await Assert.That(run.Options.Select(option => option.SwitchName)).Contains("--lifetime");
        await Assert.That(run.PositionalArguments.Single(argument => argument.PropertyName == "Args").IsVariadic).IsTrue();
    }

    [Test]
    public async Task Conflicting_Inherited_Arity_Is_Rejected_Before_Deduplication()
    {
        var scraper = new TestScraper();
        await scraper.ScrapeAsync().ToListAsync();
        var help = Fixture("up").Replace("--verbose int", "--verbose string", StringComparison.Ordinal);
        await Assert.That(() => scraper.Parse(["pulumi", "up"], help))
            .Throws<InvalidOperationException>().And.HasMessageContaining("--verbose");
    }

    private static string Fixture(string name) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Pulumi", "3.267.0", name + ".txt"));

    private sealed class TestScraper() : PulumiCliScraper(
        new FixtureExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<PulumiCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);
        public IReadOnlyList<CliOptionDefinition> Inherited(string help) => ParseNamedOptionSection(help, "Global Flags", []);
        public Task<CliCommandDefinition?> Parse(string[] path, string help) =>
            ParseCommandAsync(path, help, ParseUsageSynopsis(path, help), CancellationToken.None);

        protected override IEnumerable<string> ExtractSubcommands(string[] path, string help) => path switch
        {
            ["pulumi"] => ["stack", "up", "env"],
            ["pulumi", "stack"] => ["list"],
            ["pulumi", "env"] => ["run"],
            _ => [],
        };
    }

    private sealed class FixtureExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = arguments switch
                {
                    "--help" => Fixture("root"),
                    "stack --help" => Fixture("stack"),
                    "stack list --help" => Fixture("stack-ls"),
                    "up --help" => Fixture("up"),
                    "env --help" => Fixture("env"),
                    "env run --help" => Fixture("env-run"),
                    "--version" => "v3.267.0",
                    _ => throw new InvalidOperationException($"Unexpected help request: {arguments}"),
                },
                StandardError = string.Empty,
            });
    }
}
