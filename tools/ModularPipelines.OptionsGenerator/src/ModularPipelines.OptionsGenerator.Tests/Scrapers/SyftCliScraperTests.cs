using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SyftCliScraperTests
{
    [Test]
    public async Task Captured_Help_Produces_Only_Persistent_Root_Settings()
    {
        var tool = await Scrape();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName))
            .IsEquivalentTo(["--config", "--profile", "--quiet", "--verbose"]);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsTrue();
        foreach (var name in new[] { "--config", "--profile" })
        {
            var option = tool.GlobalOptions.Single(option => option.SwitchName == name);
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
        }

        var verbose = tool.GlobalOptions.Single(option => option.SwitchName == "--verbose");
        await Assert.That(verbose.CSharpType).IsEqualTo("int?");
        await Assert.That(verbose.IsFlag).IsFalse();
        await Assert.That(verbose.ShortForm).IsEqualTo("-v");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--config").ShortForm).IsEqualTo("-c");
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--quiet").IsFlag).IsTrue();
        await Assert.That(tool.GlobalOptions.Single(option => option.SwitchName == "--quiet").ShortForm).IsEqualTo("-q");
    }

    [Test]
    public async Task Every_Command_Inherits_One_Copy_And_Keeps_Local_Options()
    {
        var tool = await Scrape();
        await Assert.That(tool.Commands.Count).IsEqualTo(8);
        foreach (var command in tool.Commands)
        {
            await Assert.That(command.Options.Any(option => tool.GlobalOptions.Any(global => global.SwitchName == option.SwitchName))).IsFalse();
        }

        var scan = tool.Commands.Single(command => command.FullCommand == "syft scan");
        await Assert.That(scan.Options.Any(option => option.SwitchName == "--output")).IsTrue();
        await Assert.That(scan.Options.Any(option => option.SwitchName == "--scope")).IsTrue();
        var login = tool.Commands.Single(command => command.FullCommand == "syft login");
        await Assert.That(login.Options.Single(option => option.SwitchName == "--password").IsSecret).IsTrue();
        await Assert.That(tool.Commands.Single(command => command.FullCommand == "syft config").Options.Any(option => option.SwitchName == "--load")).IsTrue();
        await Assert.That(tool.Commands.Single(command => command.FullCommand == "syft config locations").Options.Any(option => option.SwitchName == "--load")).IsFalse();

        var baseCode = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(baseCode).Contains("public virtual int? Verbose");
        var commandCode = await new OptionsClassGenerator().GenerateAsync(tool);
        await Assert.That(commandCode.All(file => !file.Content.Contains("public int? Verbose"))).IsTrue();
    }

    [Test]
    [Arguments("count", "string", "-v", "-v")]
    [Arguments("count", "count", "-v", "-x")]
    public async Task Conflicting_Inherited_Metadata_Fails_Generation(string oldType, string newType, string oldAlias, string newAlias)
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestSyftCliScraper(new FixtureExecutor(), cache);
        await foreach (var unused in scraper.ScrapeAsync())
        {
        }

        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Syft", "1.54.0", "login-help.txt"));
        help = help.Replace($"{oldAlias}, --verbose {oldType}", $"{newAlias}, --verbose {newType}", StringComparison.Ordinal);
        await Assert.That(() => scraper.Parse(help))
            .Throws<InvalidOperationException>();
    }

    private static async Task<CliToolDefinition> Scrape()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new SyftCliScraper(new FixtureExecutor(), cache, NullLogger<SyftCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return scraper.CreateToolDefinition() with { Commands = commands };
    }

    private sealed class TestSyftCliScraper(ICliCommandExecutor executor, IHelpTextCache cache)
        : SyftCliScraper(executor, cache, NullLogger<SyftCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string help) =>
            ParseCommandAsync(["syft", "login"], help, ParseUsageSynopsis(["syft", "login"], help), CancellationToken.None);
    }

    private sealed class FixtureExecutor : ICliCommandExecutor
    {
        public async Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var parts = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var path = parts.Where(part => part is not "--help" and not "-h").ToArray();
            var name = path.Length == 0 ? "root" : string.Join('-', path);
            var file = name is "version" or "--version" ? "version.txt" : name + "-help.txt";
            var text = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Syft", "1.54.0", file), cancellationToken);
            return new CliCommandResult { StandardOutput = text, StandardError = string.Empty, ExitCode = 0 };
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
