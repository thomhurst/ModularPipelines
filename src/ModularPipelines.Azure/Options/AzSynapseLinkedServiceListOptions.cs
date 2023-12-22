using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "linked-service", "list")]
public record AzSynapseLinkedServiceListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;