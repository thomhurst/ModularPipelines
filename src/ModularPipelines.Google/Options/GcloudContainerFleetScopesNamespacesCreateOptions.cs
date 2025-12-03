using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "namespaces", "create")]
public record GcloudContainerFleetScopesNamespacesCreateOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Scope
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--namespace-labels")]
    public IEnumerable<KeyValue>? NamespaceLabels { get; set; }
}