using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "managed-private-endpoints", "list")]
public record AzSynapseManagedPrivateEndpointsListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;