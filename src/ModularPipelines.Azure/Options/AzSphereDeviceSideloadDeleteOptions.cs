using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "sideload", "delete")]
public record AzSphereDeviceSideloadDeleteOptions : AzOptions
{
    [CommandSwitch("--component-id")]
    public string? ComponentId { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--except-component-ids")]
    public string? ExceptComponentIds { get; set; }
}