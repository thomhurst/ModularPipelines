using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "app", "show-quota")]
public record AzSphereDeviceAppShowQuotaOptions : AzOptions
{
    [CommandSwitch("--component-id")]
    public string? ComponentId { get; set; }

    [CommandSwitch("--device")]
    public string? Device { get; set; }
}