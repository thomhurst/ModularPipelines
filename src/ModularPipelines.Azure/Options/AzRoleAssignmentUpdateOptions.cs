using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("role", "assignment", "update")]
public record AzRoleAssignmentUpdateOptions(
[property: CliOption("--role-assignment")] string RoleAssignment
) : AzOptions;