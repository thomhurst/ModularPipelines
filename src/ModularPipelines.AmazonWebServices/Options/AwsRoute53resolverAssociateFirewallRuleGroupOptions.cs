using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "associate-firewall-rule-group")]
public record AwsRoute53resolverAssociateFirewallRuleGroupOptions(
[property: CliOption("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--priority")] int Priority,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--mutation-protection")]
    public string? MutationProtection { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}