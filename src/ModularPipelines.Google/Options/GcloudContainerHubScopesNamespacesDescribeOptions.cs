using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "namespaces", "describe")]
public record GcloudContainerHubScopesNamespacesDescribeOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Scope
) : GcloudOptions;