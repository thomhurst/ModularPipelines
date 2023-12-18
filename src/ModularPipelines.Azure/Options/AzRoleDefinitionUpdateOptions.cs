using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "definition", "update")]
public record AzRoleDefinitionUpdateOptions(
[property: CommandSwitch("--role-definition")] string RoleDefinition
) : AzOptions
{
}