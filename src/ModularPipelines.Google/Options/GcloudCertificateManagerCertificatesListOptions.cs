using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "certificates", "list")]
public record GcloudCertificateManagerCertificatesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}