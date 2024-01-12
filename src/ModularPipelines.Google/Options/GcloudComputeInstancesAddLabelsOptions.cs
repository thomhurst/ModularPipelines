using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "add-labels")]
public record GcloudComputeInstancesAddLabelsOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}