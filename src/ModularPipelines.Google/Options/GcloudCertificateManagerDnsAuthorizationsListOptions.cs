using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "dns-authorizations", "list")]
public record GcloudCertificateManagerDnsAuthorizationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}