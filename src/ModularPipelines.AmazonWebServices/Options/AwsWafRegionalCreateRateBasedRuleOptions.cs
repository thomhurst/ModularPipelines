using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf-regional", "create-rate-based-rule")]
public record AwsWafRegionalCreateRateBasedRuleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--rate-key")] string RateKey,
[property: CliOption("--rate-limit")] long RateLimit,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}