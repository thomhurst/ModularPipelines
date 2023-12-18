using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "proxy", "delete")]
public record AzSphereDeviceNetworkProxyDeleteOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}