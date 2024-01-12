using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "namespaces", "create")]
public record GcloudContainerHubScopesNamespacesCreateOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Scope
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--namespace-labels")]
    public IEnumerable<KeyValue>? NamespaceLabels { get; set; }
}