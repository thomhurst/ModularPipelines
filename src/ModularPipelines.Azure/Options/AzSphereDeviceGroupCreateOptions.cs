using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device-group", "create")]
public record AzSphereDeviceGroupCreateOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name,
[property: CliOption("--product")] string Product,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-crash-dumps-collection")]
    public bool? AllowCrashDumpsCollection { get; set; }

    [CliOption("--application-update")]
    public string? ApplicationUpdate { get; set; }

    [CliOption("--os-feed")]
    public string? OsFeed { get; set; }

    [CliOption("--regional-data-boundary")]
    public string? RegionalDataBoundary { get; set; }
}