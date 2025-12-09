using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "manufacturing-state", "update")]
public record AzSphereDeviceManufacturingStateUpdateOptions(
[property: CliOption("--state")] string State
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}