using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "proxy", "show")]
public record AzSphereDeviceNetworkProxyShowOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}