using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "update-rate-based-rule")]
public record AwsWafRegionalUpdateRateBasedRuleOptions(
[property: CommandSwitch("--rule-id")] string RuleId,
[property: CommandSwitch("--change-token")] string ChangeToken,
[property: CommandSwitch("--updates")] string[] Updates,
[property: CommandSwitch("--rate-limit")] long RateLimit
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}