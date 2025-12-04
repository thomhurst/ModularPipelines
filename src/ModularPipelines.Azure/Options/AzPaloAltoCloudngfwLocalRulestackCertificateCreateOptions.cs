using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("palo-alto", "cloudngfw", "local-rulestack", "certificate", "create")]
public record AzPaloAltoCloudngfwLocalRulestackCertificateCreateOptions(
[property: CliFlag("--certificate-self-signed")] bool CertificateSelfSigned,
[property: CliOption("--local-rulestack-name")] string LocalRulestackName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--audit-comment")]
    public string? AuditComment { get; set; }

    [CliOption("--certificate-signer-id")]
    public string? CertificateSignerId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}