using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "network", "update-interface")]
public record AzSphereDeviceNetworkUpdateInterfaceOptions(
[property: CliOption("--hardware-address")] string HardwareAddress,
[property: CliOption("--interface")] string Interface
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}