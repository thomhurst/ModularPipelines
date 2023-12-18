using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "pipeline-run", "show")]
public record AzSynapsePipelineRunShowOptions(
[property: CommandSwitch("--run-id")] string RunId,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;