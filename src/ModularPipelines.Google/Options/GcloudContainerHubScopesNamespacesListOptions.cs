using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "namespaces", "list")]
public record GcloudContainerHubScopesNamespacesListOptions(
[property: CliOption("--scope")] string Scope
) : GcloudOptions;