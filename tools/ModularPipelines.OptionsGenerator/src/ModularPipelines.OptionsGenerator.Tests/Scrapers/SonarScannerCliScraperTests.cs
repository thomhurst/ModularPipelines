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
        await Assert.That(define.CSharpType).IsEqualTo("IEnumerable<KeyValue>?");
        await Assert.That(define.IsKeyValue).IsTrue();
        await Assert.That(define.AcceptsMultipleValues).IsTrue();
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

    private sealed class TestSonarScannerCliScraper : SonarScannerCliScraper
    {
        public TestSonarScannerCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<SonarScannerCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string helpText) =>
            ParseCommandAsync(["sonar-scanner"], helpText, CancellationToken.None);
    }
}
