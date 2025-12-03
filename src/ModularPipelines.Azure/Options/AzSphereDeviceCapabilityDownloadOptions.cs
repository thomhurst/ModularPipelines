using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "capability", "download")]
public record AzSphereDeviceCapabilityDownloadOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--device-group")]
    public string? DeviceGroup { get; set; }

    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--product")]
    public string? Product { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}