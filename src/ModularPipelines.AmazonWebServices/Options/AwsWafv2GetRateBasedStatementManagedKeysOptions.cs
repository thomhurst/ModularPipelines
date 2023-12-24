using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "get-rate-based-statement-managed-keys")]
public record AwsWafv2GetRateBasedStatementManagedKeysOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--web-acl-name")] string WebAclName,
[property: CommandSwitch("--web-acl-id")] string WebAclId,
[property: CommandSwitch("--rule-name")] string RuleName
) : AwsOptions
{
    [CommandSwitch("--rule-group-rule-name")]
    public string? RuleGroupRuleName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}