using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "rbacrolebindings", "describe")]
public record GcloudContainerFleetScopesRbacrolebindingsDescribeOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Scope
) : GcloudOptions;