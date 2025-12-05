using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "list-firewall-rules")]
public record AwsRoute53resolverListFirewallRulesOptions(
[property: CliOption("--firewall-rule-group-id")] string FirewallRuleGroupId
) : AwsOptions
{
    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}