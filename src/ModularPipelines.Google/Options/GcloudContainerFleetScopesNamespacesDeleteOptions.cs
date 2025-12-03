using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "namespaces", "delete")]
public record GcloudContainerFleetScopesNamespacesDeleteOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Scope
) : GcloudOptions;