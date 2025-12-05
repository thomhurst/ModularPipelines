using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "trust-configs", "list")]
public record GcloudCertificateManagerTrustConfigsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}