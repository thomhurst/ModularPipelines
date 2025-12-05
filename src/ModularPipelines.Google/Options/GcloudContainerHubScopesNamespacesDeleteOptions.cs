using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "namespaces", "delete")]
public record GcloudContainerHubScopesNamespacesDeleteOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Scope
) : GcloudOptions;