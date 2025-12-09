using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "network", "show-diagnostics")]
public record AzSphereDeviceNetworkShowDiagnosticsOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }
}