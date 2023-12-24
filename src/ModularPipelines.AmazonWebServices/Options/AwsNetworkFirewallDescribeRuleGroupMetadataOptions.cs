using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "describe-rule-group-metadata")]
public record AwsNetworkFirewallDescribeRuleGroupMetadataOptions : AwsOptions
{
    [CommandSwitch("--rule-group-name")]
    public string? RuleGroupName { get; set; }

    [CommandSwitch("--rule-group-arn")]
    public string? RuleGroupArn { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}