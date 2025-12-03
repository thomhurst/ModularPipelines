using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "namespaces", "describe")]
public record GcloudContainerFleetScopesNamespacesDescribeOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Scope
) : GcloudOptions;