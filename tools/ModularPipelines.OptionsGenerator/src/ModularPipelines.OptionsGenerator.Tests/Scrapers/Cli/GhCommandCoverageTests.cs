using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GhCommandCoverageTests
{
    [Test]
    [Arguments("stack", true)]
    [Arguments("issue", false)]
    public async Task Only_Extension_Absence_Is_Allowed_At_The_Same_Version(string removedCommand, bool allowed)
    {
        var scraper = new GhCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<GhCliScraper>.Instance);
        var tool = scraper.CreateToolDefinition() with
        {
            ToolVersion = "gh version 2.100.0",
            Commands = [Command("stack"), Command("issue")],
        };
        var outputDirectory = Path.Combine(Path.GetTempPath(), $"gh-coverage-{Guid.NewGuid():N}");
        Directory.CreateDirectory(outputDirectory);
        try
        {
            var baseline = CommandCoverageGuard.Evaluate(tool, outputDirectory, approveShrinkage: false);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);
            var current = CommandCoverageGuard.Evaluate(tool with
            {
                Commands = [.. tool.Commands.Where(command => command.CommandParts[0] != removedCommand)],
            }, outputDirectory, approveShrinkage: false);

            if (allowed)
            {
                await Assert.That(current.Violations).IsEmpty();
            }
            else
            {
                await Assert.That(current.Violations).IsNotEmpty();
            }
            await Assert.That(current.RemovedCommands).IsEquivalentTo([$"gh {removedCommand}"]);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    private static CliCommandDefinition Command(string name) => new()
    {
        FullCommand = $"gh {name}",
        CommandParts = [name],
        ClassName = $"Gh{name}Options",
        ParentClassName = "GhOptions",
        ToolNamespacePrefix = "Gh",
        Options = [],
    };
}
