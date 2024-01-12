using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "rbacrolebindings", "describe")]
public record GcloudContainerHubScopesRbacrolebindingsDescribeOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Scope
) : GcloudOptions;