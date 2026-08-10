using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SonarScannerCliScraperTests
{
    private const string HelpText = """
        usage: sonar-scanner [options]

        Options:
         -D,--define <arg>     Define property
         -h,--help             Display help information
         -v,--version          Display version information
         -X,--debug            Produce execution debug output
        """;

    [Test]
    public async Task Help_Parses_Repeatable_Defines_And_Flags()
    {
        var command = await new TestSonarScannerCliScraper().Parse(HelpText);
        var define = command!.Options.Single(x => x.SwitchName == "--define");
        var debug = command.Options.Single(x => x.SwitchName == "--debug");

        await Assert.That(define.ShortForm).IsEqualTo("-D");
        await Assert.That(define.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
        await Assert.That(define.IsKeyValue).IsTrue();
        await Assert.That(define.AcceptsMultipleValues).IsTrue();
        await Assert.That(define.IsSecret).IsTrue();
        await Assert.That(define.SecretValueKeys).IsEquivalentTo(["sonar.token", "sonar.login"]);
        await Assert.That(debug.ShortForm).IsEqualTo("-X");
        await Assert.That(debug.IsFlag).IsTrue();
    }

    [Test]
    public async Task Common_Properties_Use_Inline_Defines_And_Mark_Token_Secret()
    {
        var command = await new TestSonarScannerCliScraper().Parse(HelpText);
        var projectKey = command!.Options.Single(x => x.PropertyName == "ProjectKey");
        var token = command.Options.Single(x => x.PropertyName == "Token");

        await Assert.That(projectKey.SwitchName).IsEqualTo("-Dsonar.projectKey");
        await Assert.That(projectKey.ValueSeparator).IsEqualTo("=");
        await Assert.That(token.SwitchName).IsEqualTo("-Dsonar.token");
        await Assert.That(token.IsSecret).IsTrue();
    }

    [Test]
    public async Task Version_Strips_Startup_Logs_And_Runner_Paths()
    {
        const string capturedOutput = """
            02:49:45.708 INFO  Scanner configuration file: /home/runner/work/_temp/cli-tools/sonar-scanner-8.0.1.6346-linux-x64/conf/sonar-scanner.properties
            02:49:45.711 INFO  Project root configuration file: NONE
            02:49:45.731 INFO  SonarScanner CLI 8.0.1.6346
            02:49:45.737 INFO  Linux 6.17.0-1020-azure amd64
            """;
        var scraper = new TestSonarScannerCliScraper(new VersionExecutor(capturedOutput));

        var version = await scraper.GetVersionAsync();

        await Assert.That(version).IsEqualTo("SonarScanner CLI 8.0.1.6346");
    }

    [Test]
    public async Task Version_Returns_Null_When_No_Stable_Identity_Is_Present()
    {
        var scraper = new TestSonarScannerCliScraper(new VersionExecutor(
            "02:49:45.708 INFO Scanner configuration file: /home/runner/work/_temp/scanner.properties"));

        var version = await scraper.GetVersionAsync();

        await Assert.That(version).IsNull();
    }

    [Test]
    public async Task Version_Finds_Stable_Identity_After_Generic_Truncation_Limit()
    {
        var output = new string('x', 600) + Environment.NewLine
                     + "02:49:45.731 INFO SonarScanner CLI 8.0.1.6346";
        var scraper = new TestSonarScannerCliScraper(new VersionExecutor(output));

        var version = await scraper.GetVersionAsync();

        await Assert.That(version).IsEqualTo("SonarScanner CLI 8.0.1.6346");
    }

    private sealed class TestSonarScannerCliScraper : SonarScannerCliScraper
    {
        public TestSonarScannerCliScraper()
            : this(new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance))
        {
        }

        public TestSonarScannerCliScraper(ICliCommandExecutor executor)
            : base(
                executor,
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<SonarScannerCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string helpText) =>
            ParseCommandAsync(["sonar-scanner"], helpText, CancellationToken.None);
    }

    private sealed class VersionExecutor(string output) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
