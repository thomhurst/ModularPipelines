using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "role", "definition", "update")]
public record AzKeyvaultRoleDefinitionUpdateOptions(
[property: CliOption("--hsm-name")] string HsmName,
[property: CliOption("--role-definition")] string RoleDefinition
) : AzOptions;