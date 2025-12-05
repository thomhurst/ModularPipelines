using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "service-attachments", "create")]
public record GcloudComputeServiceAttachmentsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--nat-subnets")] string[] NatSubnets,
[property: CliOption("--producer-forwarding-rule")] string ProducerForwardingRule
) : GcloudOptions
{
    [CliOption("--connection-preference")]
    public string? ConnectionPreference { get; set; }

    [CliOption("--consumer-accept-list")]
    public string[]? ConsumerAcceptList { get; set; }

    [CliOption("--consumer-reject-list")]
    public string[]? ConsumerRejectList { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--domain-names")]
    public string[]? DomainNames { get; set; }

    [CliFlag("--enable-proxy-protocol")]
    public bool? EnableProxyProtocol { get; set; }

    [CliOption("--nat-subnets-region")]
    public string? NatSubnetsRegion { get; set; }

    [CliOption("--producer-forwarding-rule-region")]
    public string? ProducerForwardingRuleRegion { get; set; }

    [CliFlag("--reconcile-connections")]
    public bool? ReconcileConnections { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}