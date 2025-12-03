using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "certificate", "show")]
public record AzSphereDeviceCertificateShowOptions(
[property: CliOption("--certificate")] string Certificate
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}