using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "update")]
public record GcloudComputeDisksUpdateOptions(
[property: CliArgument] string DiskName
) : GcloudOptions
{
    [CliOption("--provisioned-iops")]
    public string? ProvisionedIops { get; set; }

    [CliOption("--provisioned-throughput")]
    public string? ProvisionedThroughput { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-architecture")]
    public bool? ClearArchitecture { get; set; }

    [CliOption("--update-architecture")]
    public string? UpdateArchitecture { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}