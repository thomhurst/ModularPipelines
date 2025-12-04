using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "sideload", "delete")]
public record AzSphereDeviceSideloadDeleteOptions : AzOptions
{
    [CliOption("--component-id")]
    public string? ComponentId { get; set; }

    [CliOption("--device")]
    public string? Device { get; set; }

    [CliOption("--except-component-ids")]
    public string? ExceptComponentIds { get; set; }
}