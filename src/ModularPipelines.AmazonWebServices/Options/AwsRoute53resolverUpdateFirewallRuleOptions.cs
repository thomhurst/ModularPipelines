using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-firewall-rule")]
public record AwsRoute53resolverUpdateFirewallRuleOptions(
[property: CommandSwitch("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CommandSwitch("--firewall-domain-list-id")] string FirewallDomainListId
) : AwsOptions
{
    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--block-response")]
    public string? BlockResponse { get; set; }

    [CommandSwitch("--block-override-domain")]
    public string? BlockOverrideDomain { get; set; }

    [CommandSwitch("--block-override-dns-type")]
    public string? BlockOverrideDnsType { get; set; }

    [CommandSwitch("--block-override-ttl")]
    public int? BlockOverrideTtl { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}