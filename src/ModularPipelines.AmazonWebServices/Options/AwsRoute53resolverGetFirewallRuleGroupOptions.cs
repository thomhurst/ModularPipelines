using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "get-firewall-rule-group")]
public record AwsRoute53resolverGetFirewallRuleGroupOptions(
[property: CliOption("--firewall-rule-group-id")] string FirewallRuleGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}