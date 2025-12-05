using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "namespaces", "list")]
public record GcloudContainerFleetScopesNamespacesListOptions(
[property: CliOption("--scope")] string Scope
) : GcloudOptions;