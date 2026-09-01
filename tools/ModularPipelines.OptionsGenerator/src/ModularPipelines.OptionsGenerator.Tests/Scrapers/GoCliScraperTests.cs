using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GoCliScraperTests
{
    [Test]
    [Arguments("get", "-u", "usage: go get [-t] [-u] [build flags] [packages]")]
    [Arguments("list", "-json", "usage: go list [-f format] [-json] [-m] [packages]")]
    public async Task Optional_Value_Flags_Retain_Bare_And_Valued_Forms(
        string commandName,
        string switchName,
        string helpText)
    {
        var command = await new TestGoCliScraper().Parse(
            ["go", commandName],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == switchName);

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(option.ValueSeparator).IsEqualTo("=");
            await Assert.That(option.PropertyType).IsEqualTo("CliOptionValue?");
        }
    }

    private sealed class TestGoCliScraper : GoCliScraper
    {
        public TestGoCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<GoCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
    }
}
