using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "namespaces", "update")]
public record GcloudContainerHubScopesNamespacesUpdateOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Scope
) : GcloudOptions
{
    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--update-namespace-labels")]
    public IEnumerable<KeyValue>? UpdateNamespaceLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-namespace-labels")]
    public bool? ClearNamespaceLabels { get; set; }

    [CommandSwitch("--remove-namespace-labels")]
    public string[]? RemoveNamespaceLabels { get; set; }
}