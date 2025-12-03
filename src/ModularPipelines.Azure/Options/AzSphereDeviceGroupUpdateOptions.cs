using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device-group", "update")]
public record AzSphereDeviceGroupUpdateOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--device-group")] string DeviceGroup,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-crash-dumps-collection")]
    public bool? AllowCrashDumpsCollection { get; set; }

    [CliOption("--application-update")]
    public string? ApplicationUpdate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--os-feed")]
    public string? OsFeed { get; set; }

    [CliOption("--regional-data-boundary")]
    public string? RegionalDataBoundary { get; set; }
}