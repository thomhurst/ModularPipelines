using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "local-rulestack", "local-rule", "create")]
public record AzPaloAltoCloudngfwLocalRulestackLocalRuleCreateOptions(
[property: CommandSwitch("--local-rulestack-name")] string LocalRulestackName,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions
{
    [CommandSwitch("--action-type")]
    public string? ActionType { get; set; }

    [CommandSwitch("--applications")]
    public string? Applications { get; set; }

    [CommandSwitch("--audit-comment")]
    public string? AuditComment { get; set; }

    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--decryption-rule-type")]
    public string? DecryptionRuleType { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--inbound-certificate")]
    public string? InboundCertificate { get; set; }

    [BooleanCommandSwitch("--negate-destination")]
    public bool? NegateDestination { get; set; }

    [BooleanCommandSwitch("--negate-source")]
    public bool? NegateSource { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--protocol-port-list")]
    public string? ProtocolPortList { get; set; }

    [CommandSwitch("--rule-state")]
    public string? RuleState { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}