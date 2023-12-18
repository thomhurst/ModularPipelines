using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device-group", "create-defaults")]
public record AzSphereDeviceGroupCreateDefaultsOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--device-group")]
    public string? DeviceGroup { get; set; }
}

