using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "add-labels")]
public record GcloudComputeDisksAddLabelsOptions(
[property: PositionalArgument] string DiskName,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}