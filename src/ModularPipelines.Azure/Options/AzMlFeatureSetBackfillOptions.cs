using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "feature-set", "backfill")]
public record AzMlFeatureSetBackfillOptions(
[property: CommandSwitch("--feature-name")] string FeatureName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--by-data-status")]
    public string? ByDataStatus { get; set; }

    [CommandSwitch("--by-job-id")]
    public string? ByJobId { get; set; }

    [CommandSwitch("--compute-resource")]
    public string? ComputeResource { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CommandSwitch("--feature-window-end-time")]
    public string? FeatureWindowEndTime { get; set; }

    [CommandSwitch("--feature-window-start-time")]
    public string? FeatureWindowStartTime { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--spark-configuration-settings")]
    public string? SparkConfigurationSettings { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}