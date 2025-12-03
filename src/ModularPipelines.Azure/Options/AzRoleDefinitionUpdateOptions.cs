using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("role", "definition", "update")]
public record AzRoleDefinitionUpdateOptions(
[property: CliOption("--role-definition")] string RoleDefinition
) : AzOptions;