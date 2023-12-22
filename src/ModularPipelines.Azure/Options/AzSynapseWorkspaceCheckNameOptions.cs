using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "workspace", "check-name")]
public record AzSynapseWorkspaceCheckNameOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;