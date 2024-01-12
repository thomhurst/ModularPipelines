using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "namespaces", "delete")]
public record GcloudContainerFleetScopesNamespacesDeleteOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Scope
) : GcloudOptions;