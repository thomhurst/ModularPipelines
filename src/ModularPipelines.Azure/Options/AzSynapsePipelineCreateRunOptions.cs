using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "pipeline", "create-run")]
public record AzSynapsePipelineCreateRunOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--is-recovery")]
    public bool? IsRecovery { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--reference-pipeline-run-id")]
    public string? ReferencePipelineRunId { get; set; }

    [CommandSwitch("--start-activity-name")]
    public string? StartActivityName { get; set; }
}