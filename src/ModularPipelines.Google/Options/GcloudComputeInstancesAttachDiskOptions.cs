using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "attach-disk")]
public record GcloudComputeInstancesAttachDiskOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--disk")] string Disk
) : GcloudOptions
{
    [BooleanCommandSwitch("--boot")]
    public bool? Boot { get; set; }

    [CommandSwitch("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--disk-scope")]
    public string? DiskScope { get; set; }

    [BooleanCommandSwitch("--force-attach")]
    public bool? ForceAttach { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}