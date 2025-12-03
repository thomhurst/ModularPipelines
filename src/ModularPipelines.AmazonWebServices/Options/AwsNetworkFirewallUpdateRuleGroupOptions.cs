using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "update-rule-group")]
public record AwsNetworkFirewallUpdateRuleGroupOptions(
[property: CliOption("--update-token")] string UpdateToken
) : AwsOptions
{
    [CliOption("--rule-group-arn")]
    public string? RuleGroupArn { get; set; }

    [CliOption("--rule-group-name")]
    public string? RuleGroupName { get; set; }

    [CliOption("--rule-group")]
    public string? RuleGroup { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--source-metadata")]
    public string? SourceMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}