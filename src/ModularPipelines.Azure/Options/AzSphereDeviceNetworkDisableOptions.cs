using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "network", "disable")]
public record AzSphereDeviceNetworkDisableOptions(
[property: CliOption("--interface")] string Interface
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}