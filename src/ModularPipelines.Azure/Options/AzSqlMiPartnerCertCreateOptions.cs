using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "partner-cert", "create")]
public record AzSqlMiPartnerCertCreateOptions(
[property: CliOption("--certificate-name")] string CertificateName,
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-blob")]
    public string? PublicBlob { get; set; }
}