using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "set-disk-auto-delete")]
public record GcloudComputeInstancesSetDiskAutoDeleteOptions(
[property: PositionalArgument] string InstanceName,
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--disk")] string Disk
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-delete")]
    public bool? AutoDelete { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}