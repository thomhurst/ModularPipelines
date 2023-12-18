using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "role", "definition", "show")]
public record AzSynapseRoleDefinitionShowOptions(
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}