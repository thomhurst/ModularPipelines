using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "create-rule-group")]
public record AwsNetworkFirewallCreateRuleGroupOptions(
[property: CliOption("--rule-group-name")] string RuleGroupName,
[property: CliOption("--rule-group")] string RuleGroup,
[property: CliOption("--type")] string Type,
[property: CliOption("--capacity")] int Capacity
) : AwsOptions
{
    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CliOption("--source-metadata")]
    public string? SourceMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}