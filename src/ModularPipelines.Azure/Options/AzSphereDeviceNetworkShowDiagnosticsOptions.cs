using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "network", "show-diagnostics")]
public record AzSphereDeviceNetworkShowDiagnosticsOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }
}