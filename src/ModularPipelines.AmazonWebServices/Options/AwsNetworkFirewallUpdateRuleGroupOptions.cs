using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "update-rule-group")]
public record AwsNetworkFirewallUpdateRuleGroupOptions(
[property: CommandSwitch("--update-token")] string UpdateToken
) : AwsOptions
{
    [CommandSwitch("--rule-group-arn")]
    public string? RuleGroupArn { get; set; }

    [CommandSwitch("--rule-group-name")]
    public string? RuleGroupName { get; set; }

    [CommandSwitch("--rule-group")]
    public string? RuleGroup { get; set; }

    [CommandSwitch("--rules")]
    public string? Rules { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CommandSwitch("--source-metadata")]
    public string? SourceMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}