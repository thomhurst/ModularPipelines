using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "certificate", "update")]
public record AzIotDpsCertificateUpdateOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--etag")] string Etag,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--verified")]
    public bool? Verified { get; set; }
}