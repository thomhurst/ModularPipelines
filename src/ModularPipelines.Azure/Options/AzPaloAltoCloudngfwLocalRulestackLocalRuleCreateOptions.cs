using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "local-rulestack", "local-rule", "create")]
public record AzPaloAltoCloudngfwLocalRulestackLocalRuleCreateOptions(
[property: CliOption("--local-rulestack-name")] string LocalRulestackName,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions
{
    [CliOption("--action-type")]
    public string? ActionType { get; set; }

    [CliOption("--applications")]
    public string? Applications { get; set; }

    [CliOption("--audit-comment")]
    public string? AuditComment { get; set; }

    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--decryption-rule-type")]
    public string? DecryptionRuleType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--inbound-certificate")]
    public string? InboundCertificate { get; set; }

    [CliFlag("--negate-destination")]
    public bool? NegateDestination { get; set; }

    [CliFlag("--negate-source")]
    public bool? NegateSource { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--protocol-port-list")]
    public string? ProtocolPortList { get; set; }

    [CliOption("--rule-state")]
    public string? RuleState { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}