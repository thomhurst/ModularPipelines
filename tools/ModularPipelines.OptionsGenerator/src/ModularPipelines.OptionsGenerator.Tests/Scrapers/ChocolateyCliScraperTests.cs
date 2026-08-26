using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class ChocolateyCliScraperTests
{
    [Test]
    public async Task Config_Alternatives_Become_An_Optional_Action()
    {
        const string helpText = """
            Chocolatey v2.5.1
            Config Command

            Usage

                choco config [list]|get|set|unset [<options/switches>]

            Options and Switches
            ====================

                --name=VALUE
                Name - The configuration setting name.
            """;
        var command = await new TestChocolateyCliScraper().Parse(
            ["choco", "config"],
            helpText);

        command!.ValidateOperandCoverage();

        var action = command.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(action.PropertyName).IsEqualTo("Action");
            await Assert.That(action.CSharpType).IsEqualTo("string?");
            await Assert.That(action.IsRequired).IsFalse();
            await Assert.That(command.Options.Single().PropertyName).IsEqualTo("Name");
        }
    }

    private sealed class TestChocolateyCliScraper : ChocolateyCliScraper
    {
        public TestChocolateyCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<ChocolateyCliScraper>.Instance)
        {
        }

        public async Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return await ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
