using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "describe-rule-group")]
public record AwsNetworkFirewallDescribeRuleGroupOptions : AwsOptions
{
    [CliOption("--rule-group-name")]
    public string? RuleGroupName { get; set; }

    [CliOption("--rule-group-arn")]
    public string? RuleGroupArn { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}