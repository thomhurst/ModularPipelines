using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "namespaces", "describe")]
public record GcloudContainerFleetScopesNamespacesDescribeOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Scope
) : GcloudOptions;