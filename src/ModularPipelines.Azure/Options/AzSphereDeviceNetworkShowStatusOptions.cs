using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "network", "show-status")]
public record AzSphereDeviceNetworkShowStatusOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}