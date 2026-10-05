using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class TrivyGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--cacert", "--cache-dir", "--config", "--debug", "--generate-default-config",
        "--insecure", "--quiet", "--timeout",
    ];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Root_Help_Preserves_Persistent_Options_Without_Root_Controls(string newline)
    {
        var options = new TestScraper().Globals(Fixture("root").ReplaceLineEndings(newline));

        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        foreach (var option in options)
        {
            var isFlag = option.SwitchName is "--debug" or "--generate-default-config" or "--insecure" or "--quiet";
            await Assert.That(option.IsFlag).IsEqualTo(isFlag);
            await Assert.That(option.CSharpType).IsEqualTo(isFlag ? "bool?" : "string?");
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
        }

        await Assert.That(options.Single(option => option.SwitchName == "--config").ShortForm).IsEqualTo("-c");
        await Assert.That(options.Single(option => option.SwitchName == "--debug").ShortForm).IsEqualTo("-d");
        await Assert.That(options.Single(option => option.SwitchName == "--quiet").ShortForm).IsEqualTo("-q");
    }

    [Test]
    [Arguments("image")]
    [Arguments("plugin")]
    [Arguments("plugin-install")]
    [Arguments("registry-login")]
    public async Task Captured_Leaf_And_Group_Help_Preserve_Global_Shapes(string fixture)
    {
        var scraper = new TestScraper();
        var globals = scraper.Globals(Fixture("root"));
        var command = (await scraper.Parse(["trivy", .. fixture.Split('-')], Fixture(fixture)))!;
        var inherited = command.Options.Where(option => GlobalSwitches.Contains(option.SwitchName)).ToList();

        await Assert.That(inherited.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        await Assert.That(CliGlobalOptionMerger.Merge(globals, inherited)).Count().IsEqualTo(8);
        if (fixture == "registry-login")
        {
            var password = command.Options.Single(option => option.SwitchName == "--password");
            await Assert.That(password.IsSecret).IsTrue();
            await Assert.That(password.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Discovered_Globals_Remove_Leaf_Duplicates_And_Preserve_Local_Format()
    {
        var scraper = new TestScraper(new HelpExecutor());
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition();

        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        var image = commands.Single(command => command.CommandParts.SequenceEqual(["image"]));
        await Assert.That(image.Options.Any(option => GlobalSwitches.Contains(option.SwitchName))).IsFalse();
        await Assert.That(image.Options.Any(option => option.SwitchName == "--format")).IsTrue();
        await Assert.That(tool.GlobalOptions.Any(option => option.SwitchName == "--format")).IsFalse();
    }

    [Test]
    public async Task Conflicting_Leaf_Global_Shape_Is_Not_Silently_Discarded()
    {
        var scraper = new TestScraper(new HelpExecutor());
        await scraper.ScrapeAsync().ToListAsync();
        var help = Fixture("image").Replace("--timeout duration", "--timeout int", StringComparison.Ordinal);

        await Assert.That(async () => await scraper.Parse(["trivy", "image"], help))
            .Throws<InvalidOperationException>();
    }

    private static string Fixture(string name) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Trivy", "0.75.0", name + ".txt"));

    private sealed class TestScraper(ICliCommandExecutor? executor = null) : TrivyCliScraper(
        executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<TrivyCliScraper>.Instance)
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
            var help = arguments switch
            {
                "--help" => "Usage:\n  trivy [command]\n\nAvailable Commands:\n  image   Scan an image\n\n"
                              + root[root.IndexOf("Flags:", StringComparison.Ordinal)..],
                "image --help" => Fixture("image"),
                "--version" => "Version: 0.75.0",
                _ => throw new InvalidOperationException($"Unexpected arguments: {arguments}"),
            };
            return Task.FromResult(new CliCommandResult { StandardOutput = help, StandardError = string.Empty, ExitCode = 0 });
        }
    }
}
