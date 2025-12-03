using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "network", "enable")]
public record AzSphereDeviceNetworkEnableOptions(
[property: CliOption("--interface")] string Interface
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}