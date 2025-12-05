using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "delete-firewall-rule-group")]
public record AwsRoute53resolverDeleteFirewallRuleGroupOptions(
[property: CliOption("--firewall-rule-group-id")] string FirewallRuleGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}