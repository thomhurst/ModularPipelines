using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "disassociate-firewall-rule-group")]
public record AwsRoute53resolverDisassociateFirewallRuleGroupOptions(
[property: CommandSwitch("--firewall-rule-group-association-id")] string FirewallRuleGroupAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}