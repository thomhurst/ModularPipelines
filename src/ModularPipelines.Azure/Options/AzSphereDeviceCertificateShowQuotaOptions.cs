using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "certificate", "show-quota")]
public record AzSphereDeviceCertificateShowQuotaOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}