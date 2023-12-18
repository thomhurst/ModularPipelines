using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "capability", "download")]
public record AzSphereDeviceCapabilityDownloadOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--device-group")]
    public string? DeviceGroup { get; set; }

    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }

    [CommandSwitch("--product")]
    public string? Product { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}