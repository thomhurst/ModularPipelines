using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "app", "show-memory-stats")]
public record AzSphereDeviceAppShowMemoryStatsOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}