using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GrypeCliScraperTests
{
    [Test]
    public async Task Root_Help_Exposes_Only_Verified_Persistent_Settings()
    {
        var globals = new TestScraper().ParseGlobals(Fixture("grype-help.txt"));
        await Assert.That(globals.Select(option => option.SwitchName))
            .IsEquivalentTo(["--config", "--profile", "--quiet", "--verbose"]);
        await Assert.That(globals.Single(option => option.SwitchName == "--config").CSharpType)
            .IsEqualTo("IEnumerable<string>?");
        await Assert.That(globals.Single(option => option.SwitchName == "--config").ShortForm).IsEqualTo("-c");
        await Assert.That(globals.Single(option => option.SwitchName == "--profile").CSharpType)
            .IsEqualTo("IEnumerable<string>?");
        await Assert.That(globals.Single(option => option.SwitchName == "--quiet").IsFlag).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--quiet").ShortForm).IsEqualTo("-q");
        await Assert.That(globals.Single(option => option.SwitchName == "--verbose").CSharpType).IsEqualTo("int?");
        await Assert.That(globals.Single(option => option.SwitchName == "--verbose").ShortForm).IsEqualTo("-v");
        await Assert.That(globals.All(option => !option.IsSecret)).IsTrue();
    }

    [Test]
    public async Task Traversal_Deduplicates_Inherited_Settings_And_Retains_Local_Output()
    {
        var scraper = new TestScraper();
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var status = commands.Single(command => command.CommandParts.SequenceEqual(["db", "status"]));
        await Assert.That(status.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--output"]);
        await Assert.That(status.Options.Single().CSharpType).IsEqualTo("string?");
        var search = commands.Single(command => command.CommandParts.SequenceEqual(["db", "search"]));
        await Assert.That(search.Options.Select(option => (option.SwitchName, option.CSharpType))).IsEquivalentTo([
            ("--broad-cpe-matching", "bool?"), ("--distro", "IEnumerable<string>?"),
            ("--ecosystem", "string?"), ("--fixed-state", "IEnumerable<string>?"),
            ("--limit", "int?"), ("--modified-after", "string?"), ("--output", "string?"),
            ("--pkg", "IEnumerable<string>?"), ("--provider", "IEnumerable<string>?"),
            ("--published-after", "string?"), ("--vuln", "IEnumerable<string>?")]);
        var vulnerabilities = commands.Single(command => command.CommandParts.SequenceEqual(["db", "search", "vuln"]));
        await Assert.That(vulnerabilities.Options.Select(option => (option.SwitchName, option.CSharpType))).IsEquivalentTo([
            ("--fixed-state", "IEnumerable<string>?"), ("--limit", "int?"),
            ("--modified-after", "string?"), ("--output", "string?"),
            ("--provider", "IEnumerable<string>?"), ("--published-after", "string?")]);
        await Assert.That(vulnerabilities.Options.Single(option => option.SwitchName == "--output").ShortForm).IsEqualTo("-o");
        await Assert.That(commands.Select(command => string.Join(" ", command.CommandParts)))
            .IsEquivalentTo(["db", "db status", "db search", "db search vuln"]);
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var baseCode = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(baseCode).Contains("[CliGlobalOptions]");
        await Assert.That(baseCode).Contains("public virtual IEnumerable<string>? Config");
        await Assert.That(baseCode).Contains("public virtual int? Verbose");
        var generated = await new OptionsClassGenerator().GenerateAsync(tool);
        await Assert.That(generated.All(file => !file.Content.Contains("public bool? Quiet", StringComparison.Ordinal))).IsTrue();
    }

    private static string Fixture(string name) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Grype", "0.120.0", name));

    [Test]
    public async Task Conflicting_Inherited_Arity_Is_Not_Silently_Discarded()
    {
        var scraper = new TestScraper();
        await foreach (var unused in scraper.ScrapeAsync())
        {
        }

        var help = Fixture("grype-db-status-help.txt")
            .Replace("--config stringArray", "--config string", StringComparison.Ordinal);
        await Assert.That(() => scraper.Parse(["grype", "db", "status"], help))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("--config");
    }

    private sealed class TestScraper() : GrypeCliScraper(
        new FixtureExecutor(),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<GrypeCliScraper>.Instance)
    {
        public IReadOnlyList<CliOptionDefinition> ParseGlobals(string help) => ParseGlobalOptions(help);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string help) =>
            ParseCommandAsync(commandPath, help, ParseUsageSynopsis(commandPath, help), CancellationToken.None);

        protected override IEnumerable<string> ExtractSubcommands(string[] commandPath, string helpText) =>
            commandPath switch
            {
                ["grype"] => ["db"],
                ["grype", "db"] => ["status", "search"],
                ["grype", "db", "search"] => ["vuln"],
                _ => [],
            };
    }

    private sealed class FixtureExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = arguments switch
                {
                    "--help" => Fixture("grype-help.txt"),
                    "db --help" => Fixture("grype-db-help.txt"),
                    "db status --help" => Fixture("grype-db-status-help.txt"),
                    "db search --help" => Fixture("grype-db-search-help.txt"),
                    "db search vuln --help" => Fixture("grype-db-search-vuln-help.txt"),
                    "--version" => "grype 0.120.0",
                    _ => string.Empty,
                },
                StandardError = string.Empty,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
