using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "create-rule-group")]
public record AwsNetworkFirewallCreateRuleGroupOptions(
[property: CommandSwitch("--rule-group-name")] string RuleGroupName,
[property: CommandSwitch("--rule-group")] string RuleGroup,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--capacity")] int Capacity
) : AwsOptions
{
    [CommandSwitch("--rules")]
    public string? Rules { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CommandSwitch("--source-metadata")]
    public string? SourceMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}