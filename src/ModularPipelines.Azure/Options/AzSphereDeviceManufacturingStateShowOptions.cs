using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "manufacturing-state", "show")]
public record AzSphereDeviceManufacturingStateShowOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}