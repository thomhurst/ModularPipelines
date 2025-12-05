using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "certificate", "show")]
public record AzIotDpsCertificateShowOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--dps-name")] string DpsName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}