using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "restart")]
public record AzSphereDeviceRestartOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}