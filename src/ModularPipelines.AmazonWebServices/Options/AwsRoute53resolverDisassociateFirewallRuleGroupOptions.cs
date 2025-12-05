using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "disassociate-firewall-rule-group")]
public record AwsRoute53resolverDisassociateFirewallRuleGroupOptions(
[property: CliOption("--firewall-rule-group-association-id")] string FirewallRuleGroupAssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}