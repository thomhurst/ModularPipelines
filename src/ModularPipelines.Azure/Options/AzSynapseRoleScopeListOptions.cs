using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "role", "scope", "list")]
public record AzSynapseRoleScopeListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}