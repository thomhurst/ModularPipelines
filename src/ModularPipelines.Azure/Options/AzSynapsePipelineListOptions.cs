using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "pipeline", "list")]
public record AzSynapsePipelineListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;