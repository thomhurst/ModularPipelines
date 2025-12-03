using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "set-disk-auto-delete")]
public record GcloudComputeInstancesSetDiskAutoDeleteOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--disk")] string Disk
) : GcloudOptions
{
    [CliFlag("--auto-delete")]
    public bool? AutoDelete { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}