using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "show-status")]
public record AzSphereDeviceNetworkShowStatusOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}