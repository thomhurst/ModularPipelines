using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "definition", "update")]
public record AzKeyvaultRoleDefinitionUpdateOptions(
[property: CommandSwitch("--hsm-name")] string HsmName,
[property: CommandSwitch("--role-definition")] string RoleDefinition
) : AzOptions;