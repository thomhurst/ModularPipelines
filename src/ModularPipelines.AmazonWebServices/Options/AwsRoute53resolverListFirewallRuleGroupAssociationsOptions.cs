using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "list-firewall-rule-group-associations")]
public record AwsRoute53resolverListFirewallRuleGroupAssociationsOptions : AwsOptions
{
    [CliOption("--firewall-rule-group-id")]
    public string? FirewallRuleGroupId { get; set; }

    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}