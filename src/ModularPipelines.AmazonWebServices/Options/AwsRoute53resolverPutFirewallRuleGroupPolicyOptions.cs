using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "put-firewall-rule-group-policy")]
public record AwsRoute53resolverPutFirewallRuleGroupPolicyOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--firewall-rule-group-policy")] string FirewallRuleGroupPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}