using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "feature-set", "backfill")]
public record AzMlFeatureSetBackfillOptions : AzOptions
{
    [CliOption("--by-data-status")]
    public string? ByDataStatus { get; set; }

    [CliOption("--by-job-id")]
    public string? ByJobId { get; set; }

    [CliOption("--compute-resource")]
    public string? ComputeResource { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--feature-store-name")]
    public string? FeatureStoreName { get; set; }

    [CliOption("--feature-window-end-time")]
    public string? FeatureWindowEndTime { get; set; }

    [CliOption("--feature-window-start-time")]
    public string? FeatureWindowStartTime { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--spark-configuration-settings")]
    public string? SparkConfigurationSettings { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}