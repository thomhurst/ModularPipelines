using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "app", "show-status")]
public record AzSphereDeviceAppShowStatusOptions : AzOptions
{
    [CommandSwitch("--component-id")]
    public string? ComponentId { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }
}