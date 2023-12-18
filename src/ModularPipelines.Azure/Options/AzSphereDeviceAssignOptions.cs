using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "assign")]
public record AzSphereDeviceAssignOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--targeted-device-group")] string TargetedDeviceGroup
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--device-group")]
    public string? DeviceGroup { get; set; }

    [CommandSwitch("--product")]
    public string? Product { get; set; }
}