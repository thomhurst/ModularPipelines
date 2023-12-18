using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "list")]
public record AzSphereDeviceListOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--device-group")]
    public string? DeviceGroup { get; set; }

    [CommandSwitch("--product")]
    public string? Product { get; set; }
}