using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "service-attachments", "create")]
public record GcloudComputeServiceAttachmentsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--nat-subnets")] string[] NatSubnets,
[property: CommandSwitch("--producer-forwarding-rule")] string ProducerForwardingRule
) : GcloudOptions
{
    [CommandSwitch("--connection-preference")]
    public string? ConnectionPreference { get; set; }

    [CommandSwitch("--consumer-accept-list")]
    public string[]? ConsumerAcceptList { get; set; }

    [CommandSwitch("--consumer-reject-list")]
    public string[]? ConsumerRejectList { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--domain-names")]
    public string[]? DomainNames { get; set; }

    [BooleanCommandSwitch("--enable-proxy-protocol")]
    public bool? EnableProxyProtocol { get; set; }

    [CommandSwitch("--nat-subnets-region")]
    public string? NatSubnetsRegion { get; set; }

    [CommandSwitch("--producer-forwarding-rule-region")]
    public string? ProducerForwardingRuleRegion { get; set; }

    [BooleanCommandSwitch("--reconcile-connections")]
    public bool? ReconcileConnections { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}