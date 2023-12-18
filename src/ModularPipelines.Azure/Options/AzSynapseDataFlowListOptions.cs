using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "data-flow", "list")]
public record AzSynapseDataFlowListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;