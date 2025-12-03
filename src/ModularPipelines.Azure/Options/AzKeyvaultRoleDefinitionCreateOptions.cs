using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "role", "definition", "create")]
public record AzKeyvaultRoleDefinitionCreateOptions(
[property: CliOption("--hsm-name")] string HsmName,
[property: CliOption("--role-definition")] string RoleDefinition
) : AzOptions;