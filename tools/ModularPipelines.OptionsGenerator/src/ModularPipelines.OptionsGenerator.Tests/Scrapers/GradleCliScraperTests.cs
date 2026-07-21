using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GradleCliScraperTests
{
    private const string HelpText = """
        USAGE: gradle [option...] [task...]

        Help:
        -?, -h, --help                     Shows this help message.
          --show-version, -V                 Prints version information and continues.

        Performance:
        --max-workers                      Configures the maximum number of concurrent workers Gradle is allowed to use.
        --console                          Specifies which type of console output to generate. Supported values are 'plain',
                                           'auto' (default), or 'rich'.
        -P, --project-prop                 Sets a project property for the build script (for example, -Pmyprop=myvalue).
        --watch-fs                         Enables file system watching.
        """;

    [Test]
    public async Task Parses_Actual_Gradle_Help_Format_And_Explicit_Value_Types()
    {
        var command = await new TestGradleCliScraper().Parse(HelpText);

        await Assert.That(command.Options).Count().IsEqualTo(6);

        var help = command.Options.Single(x => x.PropertyName == "Help");
        await Assert.That(help.ShortForm).IsEqualTo("-h");
        await Assert.That(help.IsFlag).IsTrue();

        var showVersion = command.Options.Single(x => x.PropertyName == "ShowVersion");
        await Assert.That(showVersion.ShortForm).IsEqualTo("-V");
        await Assert.That(showVersion.IsFlag).IsTrue();

        var maxWorkers = command.Options.Single(x => x.PropertyName == "MaxWorkers");
        await Assert.That(maxWorkers.CSharpType).IsEqualTo("int?");
        await Assert.That(maxWorkers.IsFlag).IsFalse();

        var console = command.Options.Single(x => x.PropertyName == "Console");
        await Assert.That(console.CSharpType).IsEqualTo("GradleConsole?");
        await Assert.That(console.Description).Contains("'rich'");
        await Assert.That(console.EnumDefinition!.Values.Select(x => x.CliValue)).IsEquivalentTo(["plain", "auto", "rich"]);

        var projectProperty = command.Options.Single(x => x.PropertyName == "ProjectProp");
        await Assert.That(projectProperty.CSharpType).IsEqualTo("IEnumerable<KeyValue>?");
        await Assert.That(projectProperty.AcceptsMultipleValues).IsTrue();
        await Assert.That(projectProperty.IsKeyValue).IsTrue();
        await Assert.That(command.Options.Single(x => x.PropertyName == "WatchFs").IsFlag).IsTrue();
    }

    [Test]
    public async Task Adds_Optional_Tasks_After_Options()
    {
        var command = await new TestGradleCliScraper().Parse(HelpText);
        var argument = command.PositionalArguments.Single();

        await Assert.That(argument.PropertyName).IsEqualTo("Tasks");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(argument.IsRequired).IsFalse();
        await Assert.That(argument.Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);
    }

    private sealed class TestGradleCliScraper : GradleCliScraper
    {
        public TestGradleCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<GradleCliScraper>.Instance)
        {
        }

        public async Task<CliCommandDefinition> Parse(string helpText) =>
            (await ParseCommandAsync([ToolName], helpText, CancellationToken.None))!;
    }
}
