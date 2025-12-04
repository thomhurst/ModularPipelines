using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "enable-development")]
public record AzSphereDeviceEnableDevelopmentOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--device-group")]
    public string? DeviceGroup { get; set; }

    [CliFlag("--enable-rt-core-debugging")]
    public bool? EnableRtCoreDebugging { get; set; }

    [CliOption("--product")]
    public string? Product { get; set; }
}