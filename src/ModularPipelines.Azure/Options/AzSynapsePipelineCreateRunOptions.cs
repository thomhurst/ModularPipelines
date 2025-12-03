using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "pipeline", "create-run")]
public record AzSynapsePipelineCreateRunOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--is-recovery")]
    public bool? IsRecovery { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--reference-pipeline-run-id")]
    public string? ReferencePipelineRunId { get; set; }

    [CliOption("--start-activity-name")]
    public string? StartActivityName { get; set; }
}