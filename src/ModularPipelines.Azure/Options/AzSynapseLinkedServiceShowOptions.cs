using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "linked-service", "show")]
public record AzSynapseLinkedServiceShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;