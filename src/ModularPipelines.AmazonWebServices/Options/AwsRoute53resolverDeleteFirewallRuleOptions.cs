using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "delete-firewall-rule")]
public record AwsRoute53resolverDeleteFirewallRuleOptions(
[property: CommandSwitch("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CommandSwitch("--firewall-domain-list-id")] string FirewallDomainListId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}