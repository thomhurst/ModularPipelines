using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class BrewCliScraperTests
{
    [Test]
    public async Task Traversal_Uses_Complete_Quiet_Command_Inventory()
    {
        var scraper = new TestBrewCliScraper(new CommandInventoryExecutor());
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(static command => command.FullCommand))
            .IsEquivalentTo(
            [
                "brew alpha",
                "brew baz.qux",
                "brew beta",
                "brew foo+bar",
                "brew update",
            ]);
    }

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
        public TestBrewCliScraper(ICliCommandExecutor? executor = null)
            : base(
                executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<BrewCliScraper>.Instance)
        {
        }

        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class CommandInventoryExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = arguments switch
                {
                    "--help" => "Example usage:\n  brew update",
                    "commands --quiet" => "alpha  beta  foo+bar  baz.qux\n",
                    _ => $"Usage: brew {arguments[..^7]} [options]\n\n  --verbose  Show details.",
                },
                StandardError = string.Empty,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
