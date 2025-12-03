using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "network", "show-status")]
public record AzSphereDeviceNetworkShowStatusOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}