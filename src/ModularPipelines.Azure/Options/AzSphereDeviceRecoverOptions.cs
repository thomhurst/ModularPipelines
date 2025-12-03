using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "recover")]
public record AzSphereDeviceRecoverOptions : AzOptions
{
    [CliOption("--capability")]
    public string? Capability { get; set; }

    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--images")]
    public string? Images { get; set; }
}