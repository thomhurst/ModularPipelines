using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "local-rulestack", "certificate", "create")]
public record AzPaloAltoCloudngfwLocalRulestackCertificateCreateOptions(
[property: BooleanCommandSwitch("--certificate-self-signed")] bool CertificateSelfSigned,
[property: CommandSwitch("--local-rulestack-name")] string LocalRulestackName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--audit-comment")]
    public string? AuditComment { get; set; }

    [CommandSwitch("--certificate-signer-id")]
    public string? CertificateSignerId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

