using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "add-labels")]
public record GcloudComputeDisksAddLabelsOptions(
[property: CliArgument] string DiskName,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}