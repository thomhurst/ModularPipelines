using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "list-interfaces")]
public record AzSphereDeviceNetworkListInterfacesOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}