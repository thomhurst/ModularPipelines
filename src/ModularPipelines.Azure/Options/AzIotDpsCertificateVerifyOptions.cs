using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps", "certificate", "verify")]
public record AzIotDpsCertificateVerifyOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--etag")] string Etag,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}