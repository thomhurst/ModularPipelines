using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "role", "definition", "list")]
public record AzSynapseRoleDefinitionListOptions(
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--is-built-in")]
    public bool? IsBuiltIn { get; set; }
}