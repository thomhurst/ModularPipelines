using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "delete-firewall-rule-group")]
public record AwsRoute53resolverDeleteFirewallRuleGroupOptions(
[property: CommandSwitch("--firewall-rule-group-id")] string FirewallRuleGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}