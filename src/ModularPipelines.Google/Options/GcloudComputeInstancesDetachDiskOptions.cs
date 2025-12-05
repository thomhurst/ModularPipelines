using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "detach-disk")]
public record GcloudComputeInstancesDetachDiskOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--disk")] string Disk
) : GcloudOptions
{
    [CliOption("--disk-scope")]
    public string? DiskScope { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}