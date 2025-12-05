using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "run", "list")]
public record AzMlRunListOptions : AzOptions
{
    [CliOption("--compute-target-name")]
    public string? ComputeTargetName { get; set; }

    [CliOption("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CliOption("--last")]
    public string? Last { get; set; }

    [CliFlag("--minimal")]
    public bool? Minimal { get; set; }

    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--parent-run-id")]
    public string? ParentRunId { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--pipeline-run-id")]
    public string? PipelineRunId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}