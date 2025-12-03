using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "maps", "list")]
public record GcloudCertificateManagerMapsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}