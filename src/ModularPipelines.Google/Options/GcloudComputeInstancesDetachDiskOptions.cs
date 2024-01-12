using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "detach-disk")]
public record GcloudComputeInstancesDetachDiskOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--disk")] string Disk
) : GcloudOptions
{
    [CommandSwitch("--disk-scope")]
    public string? DiskScope { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}