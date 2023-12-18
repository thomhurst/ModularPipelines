using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "pipeline-run", "cancel")]
public record AzSynapsePipelineRunCancelOptions(
[property: CommandSwitch("--run-id")] string RunId,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--is-recursive")]
    public bool? IsRecursive { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}