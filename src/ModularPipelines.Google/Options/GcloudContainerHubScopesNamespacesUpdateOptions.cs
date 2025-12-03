using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "namespaces", "update")]
public record GcloudContainerHubScopesNamespacesUpdateOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Scope
) : GcloudOptions
{
    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--update-namespace-labels")]
    public IEnumerable<KeyValue>? UpdateNamespaceLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--clear-namespace-labels")]
    public bool? ClearNamespaceLabels { get; set; }

    [CliOption("--remove-namespace-labels")]
    public string[]? RemoveNamespaceLabels { get; set; }
}