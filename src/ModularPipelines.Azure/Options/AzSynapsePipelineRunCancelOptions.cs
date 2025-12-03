using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "pipeline-run", "cancel")]
public record AzSynapsePipelineRunCancelOptions(
[property: CliOption("--run-id")] string RunId,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--is-recursive")]
    public bool? IsRecursive { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}