using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "certificate", "generate-verification-code")]
public record AzIotDpsCertificateGenerateVerificationCodeOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--etag")] string Etag
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}