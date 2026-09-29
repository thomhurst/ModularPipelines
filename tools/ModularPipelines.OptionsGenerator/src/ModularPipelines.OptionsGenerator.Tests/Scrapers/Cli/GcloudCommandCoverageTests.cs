using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudCommandCoverageTests
{
    // Removed by Google Cloud SDK 587.0.0 (release notes, 2026-09-29).
    private static readonly string[] Sdk587RemovedCommands =
    [
        "gcloud edge-cloud container vpn-connections",
        "gcloud edge-cloud container vpn-connections create",
        "gcloud edge-cloud container vpn-connections delete",
        "gcloud edge-cloud container vpn-connections describe",
        "gcloud edge-cloud container vpn-connections list",
        "gcloud storage buckets anywhere-caches pause",
    ];

    private static readonly string[] Sdk587RetainedCommands =
    [
        "gcloud edge-cloud container",
        "gcloud edge-cloud container clusters",
        "gcloud storage buckets anywhere-caches",
        "gcloud storage buckets anywhere-caches resume",
    ];

    [Test]
    public async Task Sdk_587_Upstream_Removals_Pass_Without_Blanket_Approval()
    {
        var outputDirectory = Path.Combine(Path.GetTempPath(), "mp-gcloud-coverage-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputDirectory);
        try
        {
            var tool = CreateScraper().CreateToolDefinition();
            var baseline = CommandCoverageGuard.Evaluate(
                tool with
                {
                    ToolVersion = "Google Cloud SDK 586.0.0",
                    Commands = [.. Sdk587RetainedCommands.Concat(Sdk587RemovedCommands).Select(Command)],
                },
                outputDirectory,
                approveShrinkage: false,
                allowMissingManifest: true);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            var current = CommandCoverageGuard.Evaluate(
                tool with
                {
                    ToolVersion = "Google Cloud SDK 587.0.0",
                    Commands = [.. Sdk587RetainedCommands.Select(Command)],
                },
                outputDirectory,
                approveShrinkage: false);

            using (Assert.Multiple())
            {
                await Assert.That(current.RemovedCommands).IsEquivalentTo(Sdk587RemovedCommands);
                await Assert.That(current.KnownGroupsWithoutChildren).IsEmpty();
                await Assert.That(current.Violations).IsEmpty();
            }
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Test]
    public async Task Upstream_Removal_Exclusions_Cite_The_Sdk_Release()
    {
        var exclusions = CreateScraper().CreateToolDefinition().CommandCoverage.Exclusions;

        using (Assert.Multiple())
        {
            await Assert.That(exclusions.Select(exclusion => exclusion.Command)).IsEquivalentTo(Sdk587RemovedCommands);
            await Assert.That(exclusions.All(exclusion => exclusion.Reason.Contains("Google Cloud SDK 587.0.0", StringComparison.Ordinal))).IsTrue();
        }
    }

    private static GcloudCliScraper CreateScraper() => new(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<GcloudCliScraper>.Instance);

    private static CliCommandDefinition Command(string fullCommand) => new()
    {
        FullCommand = fullCommand,
        CommandParts = [.. fullCommand.Split(' ').Skip(1)],
        ClassName = string.Concat(fullCommand.Split(' ', '-').Select(part => char.ToUpperInvariant(part[0]) + part[1..])) + "Options",
        ParentClassName = "GcloudOptions",
        ToolNamespacePrefix = "Gcloud",
        Options = [],
    };
}
