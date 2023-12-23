using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf", "update-rule-group")]
public record AwsWafUpdateRuleGroupOptions(
[property: CommandSwitch("--rule-group-id")] string RuleGroupId,
[property: CommandSwitch("--updates")] string[] Updates,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}