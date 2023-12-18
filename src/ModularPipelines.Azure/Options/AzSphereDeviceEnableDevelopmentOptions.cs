using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "enable-development")]
public record AzSphereDeviceEnableDevelopmentOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--device-group")]
    public string? DeviceGroup { get; set; }

    [BooleanCommandSwitch("--enable-rt-core-debugging")]
    public bool? EnableRtCoreDebugging { get; set; }

    [CommandSwitch("--product")]
    public string? Product { get; set; }
}