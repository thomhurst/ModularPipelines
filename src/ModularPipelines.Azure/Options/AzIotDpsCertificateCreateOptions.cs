using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps", "certificate", "create")]
public record AzIotDpsCertificateCreateOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--verified")]
    public bool? Verified { get; set; }
}