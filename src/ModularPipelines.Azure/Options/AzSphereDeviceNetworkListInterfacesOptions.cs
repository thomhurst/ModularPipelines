using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "network", "list-interfaces")]
public record AzSphereDeviceNetworkListInterfacesOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}