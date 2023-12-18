using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "assignment", "update")]
public record AzRoleAssignmentUpdateOptions(
[property: CommandSwitch("--role-assignment")] string RoleAssignment
) : AzOptions
{
}