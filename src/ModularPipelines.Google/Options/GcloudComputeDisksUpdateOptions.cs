using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "update")]
public record GcloudComputeDisksUpdateOptions(
[property: PositionalArgument] string DiskName
) : GcloudOptions
{
    [CommandSwitch("--provisioned-iops")]
    public string? ProvisionedIops { get; set; }

    [CommandSwitch("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-architecture")]
    public bool? ClearArchitecture { get; set; }

    [CommandSwitch("--update-architecture")]
    public string? UpdateArchitecture { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}