using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "role", "definition", "show")]
public record AzSynapseRoleDefinitionShowOptions(
[property: CliOption("--role")] string Role,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;