using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "certificate", "list")]
public record AzSphereDeviceCertificateListOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}