using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "delete")]
public record GcloudComputeInstancesDeleteOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--delete-disks")]
    public string? DeleteDisks { get; set; }

    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("boot")]
    public bool? Boot { get; set; }

    [BooleanCommandSwitch("data")]
    public bool? Data { get; set; }

    [CommandSwitch("--keep-disks")]
    public string? KeepDisks { get; set; }
}