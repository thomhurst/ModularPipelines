using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "create")]
public record GcloudContainerHubScopesCreateOptions(
[property: PositionalArgument] string Scope
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--namespace-labels")]
    public IEnumerable<KeyValue>? NamespaceLabels { get; set; }
}