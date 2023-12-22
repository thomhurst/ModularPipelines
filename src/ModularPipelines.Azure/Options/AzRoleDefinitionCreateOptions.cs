using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("role", "definition", "create")]
public record AzRoleDefinitionCreateOptions(
[property: CommandSwitch("--role-definition")] string RoleDefinition
) : AzOptions;