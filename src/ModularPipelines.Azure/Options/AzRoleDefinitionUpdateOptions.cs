using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "definition", "update")]
public record AzRoleDefinitionUpdateOptions(
[property: CommandSwitch("--role-definition")] string RoleDefinition
) : AzOptions;