using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "delete-firewall-rule")]
public record AwsRoute53resolverDeleteFirewallRuleOptions(
[property: CliOption("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CliOption("--firewall-domain-list-id")] string FirewallDomainListId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}