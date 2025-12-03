using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "network", "proxy", "show")]
public record AzSphereDeviceNetworkProxyShowOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}