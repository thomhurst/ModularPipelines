using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "capability", "update")]
public record AzSphereDeviceCapabilityUpdateOptions(
[property: CliOption("--capability-file")] string CapabilityFile
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}