using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "app", "show-status")]
public record AzSphereDeviceAppShowStatusOptions : AzOptions
{
    [CliOption("--component-id")]
    public string? ComponentId { get; set; }

    [CliOption("--device")]
    public string? Device { get; set; }
}