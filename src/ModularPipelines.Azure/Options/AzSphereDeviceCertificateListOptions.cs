using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "certificate", "list")]
public record AzSphereDeviceCertificateListOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}