using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CobraCliScraperTests
{
    [Test]
    public async Task Repeatable_Noun_Phrases_Produce_Collection_Options()
    {
        const string helpText = """
            Initialize a service

            Usage: fake service init [OPTIONS]

            Options:
              --external-ca external-ca   Specifications of one or more certificate signing endpoints
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "service", "init"],
            helpText);

        var option = command!.Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Short_Command_Descriptions_Are_Preserved()
    {
        const string helpText = """
            Usage: fake report usage

            Disk usage

            Options:
              --verbose   Show more details
            """;
        var command = await new TestCobraCliScraper().Parse(
            ["fake", "report", "usage"],
            helpText);

        await Assert.That(command!.Description).IsEqualTo("Disk usage");
    }

    private sealed class TestCobraCliScraper : CobraCliScraper
    {
        public TestCobraCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<TestCobraCliScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
