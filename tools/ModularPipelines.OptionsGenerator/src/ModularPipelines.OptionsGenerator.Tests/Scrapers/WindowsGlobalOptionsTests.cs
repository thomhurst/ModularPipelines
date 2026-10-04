using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class WindowsGlobalOptionsTests
{
    [Test]
    public async Task WinGet_Common_Options_Include_Aliased_Flags_Without_Root_Information_Operations()
    {
        var root = await ReadFixture("WinGet", "1.29.380", "winget-help.txt");
        var child = await ReadFixture("WinGet", "1.29.380", "winget-search-help.txt");
        root = "The following commands are available:\n  search    Search packages\n\nFor more details\n"
            + root[root.IndexOf("The following options are available:", StringComparison.Ordinal)..];
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestWinGetScraper(new FixtureExecutor(root, child), cache);
        var commands = await Scrape(scraper);
        var tool = scraper.CreateToolDefinition();
        var globals = tool.GetGlobalOptions();
        await Assert.That(globals.Select(option => option.SwitchName)).IsEquivalentTo(
            ["--wait", "--logs", "--verbose", "--nowarn", "--disable-interactivity", "--proxy", "--no-proxy"]);
        await Assert.That(globals.Where(option => option.SwitchName != "--proxy").All(option => option.IsFlag)).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--proxy").CSharpType).IsEqualTo("string?");
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        var command = commands.Single();
        await Assert.That(command.Options.Any(option => globals.Any(global => global.SwitchName == option.SwitchName))).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--cmd").IsFlag).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--source").ShortForm).IsEqualTo("-s");
    }

    [Test]
    public async Task Chocolatey_Defaults_Preserve_Value_Arity_Secrets_And_Command_First_Order()
    {
        var root = await ReadFixture("Chocolatey", "2.7.4", "choco-help.txt");
        var child = await ReadFixture("Chocolatey", "2.7.4", "choco-install-help.txt");
        root = "Commands\n * install - Install packages\n\nPlease run choco install --help\n"
            + root[root.IndexOf("Default Options and Switches", StringComparison.Ordinal)..];
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestChocolateyScraper(new FixtureExecutor(root, child), cache);
        var commands = await Scrape(scraper);
        var tool = scraper.CreateToolDefinition();
        var globals = tool.GetGlobalOptions();
        await Assert.That(globals.Single(option => option.SwitchName == "--ignore-http-cache").Description!).DoesNotContain("Chocolatey v");
        await Assert.That(globals.Single(option => option.SwitchName == "--yes").ShortForm).IsEqualTo("-y");
        await Assert.That(globals.Single(option => option.SwitchName == "--nocolor").IsFlag).IsTrue();
        await Assert.That(globals.Single(option => option.SwitchName == "--timeout").IsFlag).IsFalse();
        await Assert.That(globals.Single(option => option.SwitchName == "--cache").ValueSeparator).IsEqualTo("=");
        await Assert.That(globals.Single(option => option.SwitchName == "--proxy-password").IsSecret).IsTrue();
        await Assert.That(globals.Any(option => option.SwitchName is "--version" or "--help" or "--online")).IsFalse();
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
        var generated = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).DoesNotContain("[CliGlobalOptions]");
        var command = commands.Single();
        await Assert.That(command.Options.Any(option => globals.Any(global => global.SwitchName == option.SwitchName))).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--version").IsFlag).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--source").ShortForm).IsEqualTo("-s");
    }

    [Test]
    public async Task WinGet_Command_Override_Keeps_Its_Repeatable_Value_Shape()
    {
        const string root = "The following commands are available:\n  search    Search packages\n\nFor more details\nThe following options are available:\n  --proxy    Set proxy\n";
        const string child = "usage: winget search [<options>]\nThe following options are available:\n  --proxy    Proxy values (can be repeated)\n";
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestWinGetScraper(new FixtureExecutor(root, child), cache);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.Options.Single().AcceptsMultipleValues).IsTrue();
        await Assert.That(scraper.CreateToolDefinition().GetGlobalOptions().Single().AcceptsMultipleValues).IsFalse();
    }

    [Test]
    public async Task Chocolatey_Command_Override_Keeps_Its_Value_Arity()
    {
        const string root = "Commands\n * install - Install packages\n\nPlease run choco install -?\nDefault Options and Switches\n  --force\n    Force behavior.\n";
        const string child = "Usage\n  choco install [<options/switches>]\nOptions and Switches\n  --force=VALUE\n    A command-specific setting.\n";
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestChocolateyScraper(new FixtureExecutor(root, child), cache);
        var command = (await Scrape(scraper)).Single();
        await Assert.That(command.Options.Single().IsFlag).IsFalse();
        await Assert.That(scraper.CreateToolDefinition().GetGlobalOptions().Single().IsFlag).IsTrue();
    }

    [Test]
    public async Task WinGet_Aliased_Command_Flags_Do_Not_Require_Values()
    {
        const string root = "The following commands are available:\n  list    List packages\n\nFor more details\n";
        const string child = """
            usage: winget list [<options>]
            The following options are available:
              -r,--recurse,--all          Exports all package configurations
              -u,--unknown,--include-unknown  List packages even if their current version cannot be determined
              --pinned,--include-pinned   List packages even if they have a pin that prevents upgrade
              --asc,--ascending           Sort results in ascending order
              --desc,--descending         Sort results in descending order
              --all,--all-versions        Uninstall all versions
            """;
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestWinGetScraper(new FixtureExecutor(root, child), cache);
        var options = (await Scrape(scraper)).Single().Options;
        await Assert.That(options.Count).IsEqualTo(6);
        await Assert.That(options.All(option => option.IsFlag && option.CSharpType == "bool?")).IsTrue();
    }

    [Test]
    public async Task Chocolatey_Aliased_API_Key_Remains_Secret()
    {
        const string root = "Commands\n * apikey - Configure credentials\n\nPlease run choco apikey -?\n";
        const string child = "Usage\n  choco apikey [<options/switches>]\nOptions and Switches\n  -k, --key, --api-key=VALUE\n    ApiKey - The API key for the source.\n";
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new TestChocolateyScraper(new FixtureExecutor(root, child), cache);
        var key = (await Scrape(scraper)).Single().Options.Single();
        await Assert.That(key.SwitchName).IsEqualTo("--key");
        await Assert.That(key.ShortForm).IsEqualTo("-k");
        await Assert.That(key.IsFlag).IsFalse();
        await Assert.That(key.IsSecret).IsTrue();
    }

    private static Task<string> ReadFixture(string tool, string version, string file) =>
        File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", tool, version, file));

    private static async Task<List<CliCommandDefinition>> Scrape(CliScraperBase scraper)
    {
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return commands;
    }

    private sealed class TestWinGetScraper(ICliCommandExecutor executor, IHelpTextCache cache)
        : WinGetCliScraper(executor, cache, NullLogger<WinGetCliScraper>.Instance)
    {
        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class TestChocolateyScraper(ICliCommandExecutor executor, IHelpTextCache cache)
        : ChocolateyCliScraper(executor, cache, NullLogger<ChocolateyCliScraper>.Instance)
    {
        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class FixtureExecutor(string root, string child) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments is "--help" or "-?" ? root : child,
                StandardError = string.Empty,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
