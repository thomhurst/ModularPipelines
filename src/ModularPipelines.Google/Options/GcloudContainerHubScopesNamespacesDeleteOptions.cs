using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "namespaces", "delete")]
public record GcloudContainerHubScopesNamespacesDeleteOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Scope
) : GcloudOptions;