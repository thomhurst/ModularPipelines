using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "role", "definition", "list")]
public record AzSynapseRoleDefinitionListOptions(
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--is-built-in")]
    public bool? IsBuiltIn { get; set; }
}