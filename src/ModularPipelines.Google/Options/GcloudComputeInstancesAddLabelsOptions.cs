using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "add-labels")]
public record GcloudComputeInstancesAddLabelsOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}