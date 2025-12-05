using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-firewall-rule-group-association")]
public record AwsRoute53resolverUpdateFirewallRuleGroupAssociationOptions(
[property: CliOption("--firewall-rule-group-association-id")] string FirewallRuleGroupAssociationId
) : AwsOptions
{
    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--mutation-protection")]
    public string? MutationProtection { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}