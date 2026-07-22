using Microsoft.Extensions.Logging.Abstractions;
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
        await Assert.That(define.CSharpType).IsEqualTo("IEnumerable<KeyValue>?");
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
        await Assert.That(argument.Placement).IsEqualTo(PositionalArgumentPosition.AfterOptions);
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
    }
}
