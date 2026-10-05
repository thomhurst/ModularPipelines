using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class CosignCliScraperTests
{
    [Test]
    public async Task Root_Persistent_Options_Preserve_Types_And_Aliases()
    {
        var scraper = new TestCosignCliScraper();
        var options = scraper.ParseGlobals(CosignFixture("root"));

        await Assert.That(options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--output-file", "--timeout", "--verbose"]);
        await Assert.That(options.Single(option => option.SwitchName == "--output-file").CSharpType)
            .IsEqualTo("string?");
        var timeout = options.Single(option => option.SwitchName == "--timeout");
        await Assert.That(timeout.CSharpType).IsEqualTo("string?");
        await Assert.That(timeout.IsFlag).IsFalse();
        await Assert.That(timeout.ShortForm).IsEqualTo("-t");
        var verbose = options.Single(option => option.SwitchName == "--verbose");
        await Assert.That(verbose.IsFlag).IsTrue();
        await Assert.That(verbose.ShortForm).IsEqualTo("-d");
        await Assert.That(options.All(option => !option.IsSecret && !option.AcceptsMultipleValues)).IsTrue();
    }

    [Test]
    public async Task Captured_Commands_Inherit_One_Copy_Of_Persistent_Options()
    {
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new CosignCliScraper(new CosignFixtureExecutor(), cache, NullLogger<CosignCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        await Assert.That(commands).Count().IsEqualTo(40);
        await Assert.That(tool.GetGlobalOptions().Select(option => option.SwitchName))
            .IsEquivalentTo(["--output-file", "--timeout", "--verbose"]);
        await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsTrue();
        await Assert.That(scraper.UnavailableHelpPaths).IsEmpty();
        await Assert.That(commands.SelectMany(command => command.Options)
            .Any(option => option.SwitchName is "--output-file" or "--timeout" or "--verbose")).IsFalse();
        var sign = commands.Single(command => command.FullCommand == "cosign sign");
        await Assert.That(sign.Options.Single(option => option.SwitchName == "--identity-token").IsSecret).IsTrue();
        await Assert.That(sign.Options.Any(option => option.SwitchName == "--key")).IsTrue();
        var piv = commands.Single(command => command.FullCommand == "cosign piv-tool attestation");
        await Assert.That(piv.Options.Any(option => option.SwitchName == "--no-input")).IsTrue();
        await Assert.That(sign.Options.Any(option => option.SwitchName == "--no-input")).IsFalse();

        var baseOptions = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(baseOptions).Contains("[CliGlobalOptions]");
        await Assert.That(baseOptions).Contains("public virtual string? Timeout");
        await Assert.That(baseOptions).DoesNotContain("IdentityToken");
        var generated = await new OptionsClassGenerator().GenerateAsync(tool);
        await Assert.That(generated.All(file => !file.Content.Contains("public string? Timeout"))).IsTrue();
    }

    [Test]
    [Arguments("--timeout duration", "--timeout int")]
    [Arguments("-d, --verbose", "-v, --verbose")]
    public async Task Conflicting_Inherited_Option_Is_Rejected(string original, string replacement)
    {
        var scraper = new TestCosignCliScraper(new CosignFixtureExecutor());
        await foreach (var command in scraper.ScrapeAsync())
        {
            // Initialize the global options from the captured root help.
        }

        var help = CosignFixture("sign");
        await Assert.That(help).Contains(original);
        await Assert.That(() => scraper.Parse(["cosign", "sign"], help.Replace(original, replacement, StringComparison.Ordinal)))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("conflicting scraped and supplemental definitions");
    }

    private static string CosignFixture(string command) => File.ReadAllText(Path.Combine(
        AppContext.BaseDirectory, "Fixtures", "Cosign", "3.1.3", command.Replace(' ', '-') + ".txt"));

    private sealed class CosignFixtureExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var fixture = arguments is "version" or "--version" ? "version"
                : arguments is "--help" or "-h" ? "root"
                : string.Join(' ', arguments.Split(' ').SkipLast(1));
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = CosignFixture(fixture),
                StandardError = string.Empty,
                ExitCode = 0,
            });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
