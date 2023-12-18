using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "enable")]
public record AzSphereDeviceNetworkEnableOptions(
[property: CommandSwitch("--interface")] string Interface
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}