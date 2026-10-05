using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class YqCliScraperTests
{
    [Test]
    public async Task Persistent_Options_Are_Inherited_Without_Changing_Operands()
    {
        var scraper = new YqCliScraper(new FixtureExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<YqCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var globals = tool.GetGlobalOptions();
        await Assert.That(commands.Select(command => command.FullCommand)).IsEquivalentTo(["yq eval", "yq eval-all"]);
        await Assert.That(globals.Count).IsEqualTo(46);
        await Assert.That(globals.Any(option => option.SwitchName is "--version" or "--help")).IsFalse();
        await Assert.That(globals.Single(option => option.SwitchName == "--colors").ShortForm).IsEqualTo("-C");
        await Assert.That(globals.Single(option => option.SwitchName == "--yaml-compact-seq-indent").ShortForm).IsEqualTo("-c");
        await Assert.That(globals.Single(option => option.SwitchName == "--indent").CSharpType).IsEqualTo("int?");
        await Assert.That(globals.Single(option => option.SwitchName == "--unwrapScalar").IsFlag).IsFalse();
        await Assert.That(globals.Any(option => option.AcceptsMultipleValues || option.IsSecret)).IsFalse();
        foreach (var command in commands)
        {
            await Assert.That(command.Options).IsEmpty();
        }

        var generated = await new OptionsClassGenerator().GenerateAsync(tool);
        foreach (var file in generated.Where(file => file.Content.Contains("ExpressionArgument", StringComparison.Ordinal)))
        {
            await Assert.That(file.Content).Contains("public string? ExpressionArgument");
            await Assert.That(file.Content).DoesNotContain("public virtual string? Expression {");
        }
        await Assert.That(generated.Count(file => file.Content.Contains("ExpressionArgument", StringComparison.Ordinal))).IsEqualTo(2);
    }

    private sealed class FixtureExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var name = arguments switch
            {
                "--help" => "yq",
                "eval --help" => "yq-eval",
                "eval-all --help" => "yq-eval-all",
                _ => throw new InvalidOperationException($"Unexpected help request: {arguments}"),
            };
            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Yq", "4.54.1", name + ".txt")),
                StandardError = string.Empty,
            });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
