using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class BrewCliScraperTests
{
    [Test]
    public async Task Preserves_Positional_Operands_From_Usage()
    {
        const string helpText = """
            Usage: brew install [options] formula|cask [...]

            Install a formula or cask.

                  --formula   Treat all named arguments as formulae.
            """;

        var command = await new TestBrewCliScraper().Parse(
            ["brew", "install"],
            helpText);

        await Assert.That(command!.PositionalArguments).HasSingleItem();
        var argument = command.PositionalArguments.Single();
        var formulaOption = command.Options.Single(option => option.SwitchName == "--formula");
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("FormulaOperand");
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(formulaOption.PropertyName).IsEqualTo("Formula");
        }
    }

    private sealed class TestBrewCliScraper : BrewCliScraper
    {
        public TestBrewCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<BrewCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
