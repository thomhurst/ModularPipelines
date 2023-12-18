using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "role", "definition", "create")]
public record AzKeyvaultRoleDefinitionCreateOptions(
[property: CommandSwitch("--hsm-name")] string HsmName,
[property: CommandSwitch("--role-definition")] string RoleDefinition
) : AzOptions;