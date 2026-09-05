using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class MavenCliScraperTests
{
    private const string HelpText = """
        usage: mvn [options] [<goal(s)>] [<phase(s)>]

        Options:
         -B,--batch-mode                         Run in non-interactive (batch)
                                                 mode
            --color <arg>                        Defines the color mode. Supported are
                                                 'auto', 'always', 'never'.
         -D,--define <arg>                       Define a user property
         -emp,--encrypt-master-password <arg>   Encrypt master security password
        """;

    [Test]
    public async Task Parses_Typed_Repeatable_And_Secret_Options()
    {
        var command = await new TestMavenCliScraper().Parse(HelpText);

        await Assert.That(command.Options).Count().IsEqualTo(4);
        await Assert.That(command.Options.Single(x => x.PropertyName == "BatchMode").IsFlag).IsTrue();

        var color = command.Options.Single(x => x.PropertyName == "Color");
        await Assert.That(color.CSharpType).IsEqualTo("MavenColor?");
        await Assert.That(color.EnumDefinition!.Values.Select(x => x.CliValue)).IsEquivalentTo(["auto", "always", "never"]);

        var define = command.Options.Single(x => x.PropertyName == "Define");
        await Assert.That(define.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
        await Assert.That(define.AcceptsMultipleValues).IsTrue();
        await Assert.That(define.IsKeyValue).IsTrue();
        await Assert.That(command.Options.Single(x => x.PropertyName == "EncryptMasterPassword").IsSecret).IsTrue();
    }

    [Test]
    public async Task Adds_Optional_Goals_And_Phases_After_Options()
    {
        var command = await new TestMavenCliScraper().Parse(HelpText);
        var argument = command.PositionalArguments.Single();

        await Assert.That(argument.PropertyName).IsEqualTo("GoalsAndPhases");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(argument.IsRequired).IsFalse();
        await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    public async Task Parses_Stable_Version_Identity()
    {
        const string versionOutput = """
            Apache Maven 3.9.16 (abcdef)
            Maven home: /home/runner/work/_temp/cli-tools/apache-maven-3.9.16
            Java version: 17.0.20.1, vendor: Eclipse Adoptium
            OS name: "linux", version: "6.17.0-1022-azure", arch: "amd64"
            """;

        var version = new TestMavenCliScraper().ParseVersion(versionOutput);

        await Assert.That(version).IsEqualTo("3.9.16");
    }

    private sealed class TestMavenCliScraper : MavenCliScraper
    {
        public TestMavenCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<MavenCliScraper>.Instance)
        {
        }

        public async Task<CliCommandDefinition> Parse(string helpText) =>
            (await ParseCommandAsync([ToolName], helpText, CancellationToken.None))!;

        public string? ParseVersion(string standardOutput) => ParseVersionOutput(new CliCommandResult
        {
            StandardOutput = standardOutput,
            StandardError = string.Empty,
            ExitCode = 0,
        });
    }
}
