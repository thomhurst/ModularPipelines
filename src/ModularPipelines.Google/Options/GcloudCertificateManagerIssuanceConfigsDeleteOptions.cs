using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "issuance-configs", "delete")]
public record GcloudCertificateManagerIssuanceConfigsDeleteOptions(
[property: CliArgument] string CertificateIssuanceConfig,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}