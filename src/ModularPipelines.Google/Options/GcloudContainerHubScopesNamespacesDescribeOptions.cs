using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "namespaces", "describe")]
public record GcloudContainerHubScopesNamespacesDescribeOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Scope
) : GcloudOptions;