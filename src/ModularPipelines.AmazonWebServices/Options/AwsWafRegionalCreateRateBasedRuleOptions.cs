using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "create-rate-based-rule")]
public record AwsWafRegionalCreateRateBasedRuleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--rate-key")] string RateKey,
[property: CommandSwitch("--rate-limit")] long RateLimit,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}