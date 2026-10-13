using System.Text.Json;
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

    private static readonly string[] Sdk589RemovedCommands =
    [
        "gcloud assured v2",
        "gcloud assured v2 workloads",
        "gcloud assured v2 workloads create",
        "gcloud assured v2 workloads delete",
        "gcloud assured v2 workloads describe",
        "gcloud assured v2 workloads list",
        "gcloud assured v2 workloads update",
        "gcloud telco-automation",
        "gcloud telco-automation operations",
        "gcloud telco-automation operations describe",
        "gcloud telco-automation operations wait",
        "gcloud telco-automation orchestration-cluster",
        "gcloud telco-automation orchestration-cluster create",
        "gcloud telco-automation orchestration-cluster delete",
        "gcloud telco-automation orchestration-cluster describe",
        "gcloud telco-automation orchestration-cluster list",
    ];

    private static readonly string[] Sdk589RetainedCommands =
    [
        .. Sdk587RetainedCommands,
        "gcloud assured",
        "gcloud assured workloads",
        "gcloud assured workloads list",
        "gcloud compute instances list",
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
            await Assert.That(exclusions.Select(exclusion => exclusion.Command))
                .IsEquivalentTo(Sdk587RemovedCommands.Concat(Sdk589RemovedCommands));
            foreach (var command in Sdk587RemovedCommands)
            {
                await Assert.That(exclusions.Single(exclusion => exclusion.Command == command).Reason)
                    .Contains("Google Cloud SDK 587.0.0");
            }

            foreach (var command in Sdk589RemovedCommands)
            {
                var reason = exclusions.Single(exclusion => exclusion.Command == command).Reason;
                await Assert.That(reason).Contains("Google Cloud SDK 589.0.0");
                await Assert.That(reason).Contains("exits 2");
                await Assert.That(reason).Contains("Invalid choice");
                await Assert.That(reason).Contains("#6000");
            }
        }
    }

    [Test]
    public async Task Sdk_589_Direct_Help_Evidence_Covers_Every_Exact_Removal()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "589.0.0", "command-removal-help.json");
        using var evidence = JsonDocument.Parse(await File.ReadAllTextAsync(path));
        var root = evidence.RootElement;
        await Assert.That(root.GetProperty("sdkVersion").GetString()).IsEqualTo("589.0.0");
        var responses = root.GetProperty("responses").EnumerateArray().ToArray();
        await Assert.That(responses.Where(response => response.GetProperty("exitCode").GetInt32() != 0)
            .Select(response => response.GetProperty("commandPath").GetString()!))
            .IsEquivalentTo(Sdk589RemovedCommands);

        foreach (var command in Sdk589RemovedCommands)
        {
            var response = responses.Single(response => response.GetProperty("commandPath").GetString() == command);
            using (Assert.Multiple())
            {
                await Assert.That(response.GetProperty("arguments").GetString())
                    .IsEqualTo(string.Join(' ', command.Split(' ').Skip(1)) + " --help");
                await Assert.That(response.GetProperty("exitCode").GetInt32()).IsEqualTo(2);
                await Assert.That(response.GetProperty("standardOutput").GetString()).IsEmpty();
                await Assert.That(response.GetProperty("standardError").GetString())
                    .Contains(command.StartsWith("gcloud assured", StringComparison.Ordinal)
                        ? "Invalid choice: 'v2'" : "Invalid choice: 'telco-automation'");
            }
        }

        foreach (var command in new[] { "gcloud", "gcloud assured", "gcloud assured workloads", "gcloud assured workloads list", "gcloud compute instances list" })
        {
            var response = responses.Single(response => response.GetProperty("commandPath").GetString() == command);
            await Assert.That(response.GetProperty("exitCode").GetInt32()).IsEqualTo(0);
            await Assert.That(response.GetProperty("standardOutput").GetString()).Contains("SYNOPSIS");
        }
    }

    [Test]
    public async Task Sdk_587_To_589_Removals_Pass_Without_Blanket_Approval()
    {
        var current = await EvaluateSdk589();
        using (Assert.Multiple())
        {
            await Assert.That(current.RemovedCommands).IsEquivalentTo(Sdk589RemovedCommands);
            await Assert.That(current.KnownGroupsWithoutChildren).IsEmpty();
            await Assert.That(current.Violations).IsEmpty();
            await Assert.That(current.ChangesApproved).IsFalse();
        }
    }

    [Test]
    [Arguments("gcloud assured workloads list")]
    [Arguments("gcloud assured v2 workloads unreviewed")]
    [Arguments("gcloud telco-automation unreviewed")]
    [Arguments("gcloud compute instances list")]
    public async Task Sdk_589_Exclusions_Do_Not_Approve_Other_Removals(string command)
    {
        var current = await EvaluateSdk589(additionalBaselineCommand: command, missingCommand: command);
        await Assert.That(current.Violations.Any(violation =>
            violation.Contains("Removed commands require explicit approval", StringComparison.Ordinal)
            && violation.Contains(command, StringComparison.Ordinal))).IsTrue();
    }

    [Test]
    public async Task Sdk_589_Exclusions_Preserve_Minimum_And_Sentinel_Checks()
    {
        var current = await EvaluateSdk589(missingCommand: "gcloud assured workloads list", protectCoverage: true);
        using (Assert.Multiple())
        {
            await Assert.That(current.Violations.Any(violation => violation.Contains("below the configured minimum", StringComparison.Ordinal))).IsTrue();
            await Assert.That(current.Violations.Any(violation => violation.Contains("Missing sentinel commands: gcloud assured workloads list", StringComparison.Ordinal))).IsTrue();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Sdk_589_Exclusions_Never_Approve_Unavailable_Help(bool approveShrinkage)
    {
        var current = await EvaluateSdk589(unavailableHelpPaths: ["gcloud assured v2", "gcloud telco-automation"], approveShrinkage: approveShrinkage);
        using (Assert.Multiple())
        {
            await Assert.That(current.RemovedCommands).IsEmpty();
            await Assert.That(current.Violations.Any(violation => violation.Contains("Help was unavailable after all retries", StringComparison.Ordinal))).IsTrue();
        }
    }

    private static async Task<CommandCoverageEvaluation> EvaluateSdk589(
        string? additionalBaselineCommand = null,
        string? missingCommand = null,
        bool protectCoverage = false,
        IReadOnlyList<string>? unavailableHelpPaths = null,
        bool approveShrinkage = false)
    {
        var outputDirectory = Path.Combine(Path.GetTempPath(), "mp-gcloud-coverage-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputDirectory);
        try
        {
            var tool = CreateScraper().CreateToolDefinition();
            if (protectCoverage)
            {
                tool = tool with
                {
                    CommandCoverage = tool.CommandCoverage with
                    {
                        MinimumCommandCount = Sdk589RetainedCommands.Length,
                        SentinelCommands = ["gcloud assured workloads list"],
                    },
                };
            }

            var baselineCommands = Sdk589RetainedCommands.Concat(Sdk589RemovedCommands);
            if (additionalBaselineCommand is not null)
            {
                baselineCommands = baselineCommands.Append(additionalBaselineCommand);
            }

            var baseline = CommandCoverageGuard.Evaluate(
                tool with { ToolVersion = "Google Cloud SDK 587.0.0", Commands = [.. baselineCommands.Select(Command)] },
                outputDirectory, approveShrinkage: false, allowMissingManifest: true);
            await CommandCoverageGuard.WriteManifestAsync(baseline, CancellationToken.None);

            return CommandCoverageGuard.Evaluate(
                tool with
                {
                    ToolVersion = "Google Cloud SDK 589.0.0",
                    Commands = [.. Sdk589RetainedCommands.Where(command => command != missingCommand).Select(Command)],
                },
                outputDirectory, approveShrinkage: approveShrinkage, unavailableHelpPaths: unavailableHelpPaths);
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
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
