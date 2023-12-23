using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "get-firewall-rule-group-association")]
public record AwsRoute53resolverGetFirewallRuleGroupAssociationOptions(
[property: CommandSwitch("--firewall-rule-group-association-id")] string FirewallRuleGroupAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}