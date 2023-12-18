using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "app", "show-memory-stats")]
public record AzSphereDeviceAppShowMemoryStatsOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}