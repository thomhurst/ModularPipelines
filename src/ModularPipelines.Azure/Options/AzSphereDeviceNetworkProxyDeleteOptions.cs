using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "network", "proxy", "delete")]
public record AzSphereDeviceNetworkProxyDeleteOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}