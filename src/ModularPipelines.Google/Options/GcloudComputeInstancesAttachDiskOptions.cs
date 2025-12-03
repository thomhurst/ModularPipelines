using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "attach-disk")]
public record GcloudComputeInstancesAttachDiskOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--disk")] string Disk
) : GcloudOptions
{
    [CliFlag("--boot")]
    public bool? Boot { get; set; }

    [CliOption("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CliOption("--device-name")]
    public string? DeviceName { get; set; }

    [CliOption("--disk-scope")]
    public string? DiskScope { get; set; }

    [CliFlag("--force-attach")]
    public bool? ForceAttach { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}