using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("role", "definition", "create")]
public record AzRoleDefinitionCreateOptions(
[property: CliOption("--role-definition")] string RoleDefinition
) : AzOptions;