using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "role", "assignment", "show")]
public record AzSynapseRoleAssignmentShowOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;