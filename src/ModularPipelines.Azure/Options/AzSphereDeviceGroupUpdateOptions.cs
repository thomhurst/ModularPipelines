using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device-group", "update")]
public record AzSphereDeviceGroupUpdateOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--device-group")] string DeviceGroup,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--allow-crash-dumps-collection")]
    public bool? AllowCrashDumpsCollection { get; set; }

    [CommandSwitch("--application-update")]
    public string? ApplicationUpdate { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--os-feed")]
    public string? OsFeed { get; set; }

    [CommandSwitch("--regional-data-boundary")]
    public string? RegionalDataBoundary { get; set; }
}