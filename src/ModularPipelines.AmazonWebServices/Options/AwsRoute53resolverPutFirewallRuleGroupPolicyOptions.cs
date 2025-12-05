using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "put-firewall-rule-group-policy")]
public record AwsRoute53resolverPutFirewallRuleGroupPolicyOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--firewall-rule-group-policy")] string FirewallRuleGroupPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}