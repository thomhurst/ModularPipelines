using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "disable")]
public record AzSphereDeviceNetworkDisableOptions(
[property: CommandSwitch("--interface")] string Interface
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}