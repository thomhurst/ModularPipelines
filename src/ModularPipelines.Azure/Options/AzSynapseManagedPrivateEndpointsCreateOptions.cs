using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "managed-private-endpoints", "create")]
public record AzSynapseManagedPrivateEndpointsCreateOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--pe-name")] string PeName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;