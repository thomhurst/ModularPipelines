using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "issuance-configs", "list")]
public record GcloudCertificateManagerIssuanceConfigsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}