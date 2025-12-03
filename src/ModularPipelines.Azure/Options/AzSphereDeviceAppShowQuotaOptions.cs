using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "app", "show-quota")]
public record AzSphereDeviceAppShowQuotaOptions : AzOptions
{
    [CliOption("--component-id")]
    public string? ComponentId { get; set; }

    [CliOption("--device")]
    public string? Device { get; set; }
}