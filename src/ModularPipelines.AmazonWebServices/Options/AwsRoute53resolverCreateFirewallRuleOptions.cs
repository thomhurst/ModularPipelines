using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "create-firewall-rule")]
public record AwsRoute53resolverCreateFirewallRuleOptions(
[property: CommandSwitch("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CommandSwitch("--firewall-domain-list-id")] string FirewallDomainListId,
[property: CommandSwitch("--priority")] int Priority,
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--block-response")]
    public string? BlockResponse { get; set; }

    [CommandSwitch("--block-override-domain")]
    public string? BlockOverrideDomain { get; set; }

    [CommandSwitch("--block-override-dns-type")]
    public string? BlockOverrideDnsType { get; set; }

    [CommandSwitch("--block-override-ttl")]
    public int? BlockOverrideTtl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}