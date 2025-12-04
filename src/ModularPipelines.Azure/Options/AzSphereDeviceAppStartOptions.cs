using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "app", "start")]
public record AzSphereDeviceAppStartOptions : AzOptions
{
    [CliOption("--component-id")]
    public string? ComponentId { get; set; }

    [CliFlag("--debug-mode")]
    public bool? DebugMode { get; set; }

    [CliOption("--device")]
    public string? Device { get; set; }
}