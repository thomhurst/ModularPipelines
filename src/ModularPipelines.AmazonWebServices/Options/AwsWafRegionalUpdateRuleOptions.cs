using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "update-rule")]
public record AwsWafRegionalUpdateRuleOptions(
[property: CommandSwitch("--rule-id")] string RuleId,
[property: CommandSwitch("--change-token")] string ChangeToken,
[property: CommandSwitch("--updates")] string[] Updates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}