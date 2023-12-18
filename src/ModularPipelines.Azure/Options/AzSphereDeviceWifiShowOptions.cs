using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "wifi", "show")]
public record AzSphereDeviceWifiShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}