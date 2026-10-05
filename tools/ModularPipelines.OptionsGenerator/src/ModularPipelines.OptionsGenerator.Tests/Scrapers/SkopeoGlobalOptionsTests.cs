using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SkopeoGlobalOptionsTests
{
    private static readonly string[] GlobalSwitches =
    [
        "--command-timeout", "--debug", "--insecure-policy", "--override-arch", "--override-os",
        "--override-variant", "--policy", "--registries.d", "--tmpdir",
    ];

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Public_Persistent_Settings_Exclude_Root_Controls(string newline)
    {
        var options = new TestScraper().Globals(Fixture("root-help").ReplaceLineEndings(newline));
        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        await Assert.That(options.Single(option => option.SwitchName == "--registries.d").PropertyName).IsEqualTo("RegistriesD");
        await Assert.That(options.Single(option => option.SwitchName == "--command-timeout").CSharpType).IsEqualTo("string?");
        foreach (var option in options.Where(option => option.CSharpType == "bool?"))
        {
            await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(option.IsFlag).IsFalse();
        }
        await Assert.That(options.Any(option => option.IsSecret)).IsFalse();
    }

    [Test]
    public async Task Full_Traversal_Preserves_Local_Tls_Credentials_And_Aliases()
    {
        var scraper = new TestScraper();
        var commands = await scraper.ScrapeAsync().ToListAsync();
        var tool = scraper.CreateToolDefinition();
        await Assert.That(commands).Count().IsEqualTo(11);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsTrue();
        await Assert.That(tool.GlobalOptions.Select(option => option.SwitchName)).IsEquivalentTo(GlobalSwitches);
        await Assert.That(commands.SelectMany(command => command.Options)
            .Any(option => GlobalSwitches.Contains(option.SwitchName))).IsFalse();
        var inspect = commands.Single(command => command.FullCommand == "skopeo inspect");
        await Assert.That(inspect.Options.Single(option => option.SwitchName == "--tls-verify").ValueArity)
            .IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(inspect.Options.Single(option => option.SwitchName == "--creds").IsSecret).IsTrue();
        await Assert.That(inspect.Options.Single(option => option.SwitchName == "--authfile").IsSecret).IsFalse();
        var copy = commands.Single(command => command.FullCommand == "skopeo copy");
        foreach (var name in new[] { "--src-creds", "--dest-creds" })
        {
            await Assert.That(copy.Options.Single(option => option.SwitchName == name).IsSecret).IsTrue();
        }
        foreach (var name in new[] { "--src-tls-verify", "--dest-tls-verify" })
        {
            await Assert.That(copy.Options.Single(option => option.SwitchName == name).ValueArity)
                .IsEqualTo(CliOptionValueArity.Optional);
        }
        var login = commands.Single(command => command.FullCommand == "skopeo login");
        await Assert.That(login.Options.Single(option => option.SwitchName == "--verbose").ShortForm).IsEqualTo("-v");
        await Assert.That(login.Options.Single(option => option.SwitchName == "--password").ShortForm).IsEqualTo("-p");
    }

    [Test]
    [Arguments("string", false)]
    [Arguments("int", true)]
    public async Task Inherited_Duplicates_Are_Validated_Before_Removal(string hint, bool conflict)
    {
        var scraper = new TestScraper();
        await scraper.ScrapeAsync().ToListAsync();
        var help = $"Usage: skopeo inspect [flags] IMAGE-NAME\n\nFlags:\n  --policy {hint}    Path to a trust policy file\n";
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
        AppContext.BaseDirectory, "Fixtures", "Skopeo", "1.13.3", name + ".txt"));

    private sealed class TestScraper : SkopeoCliScraper
    {
        public TestScraper() : base(new HelpExecutor(), new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<SkopeoCliScraper>.Instance)
        {
        }

        public IReadOnlyList<CliOptionDefinition> Globals(string help) => ParseGlobalOptions(help);

        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(["skopeo", "inspect"], help,
            ParseUsageSynopsis(["skopeo", "inspect"], help), CancellationToken.None);
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
                _ when arguments.EndsWith(" --help", StringComparison.Ordinal) => arguments[..^7] + "-help",
                _ => throw new InvalidOperationException($"Unexpected help command: {arguments}"),
            };
            return Task.FromResult(new CliCommandResult { StandardOutput = Fixture(fixture), StandardError = string.Empty, ExitCode = 0 });
        }
    }
}
