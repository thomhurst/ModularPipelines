using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "definition", "update")]
public record AzKeyvaultRoleDefinitionUpdateOptions(
[property: CommandSwitch("--hsm-name")] string HsmName,
[property: CommandSwitch("--role-definition")] string RoleDefinition
) : AzOptions
{
}