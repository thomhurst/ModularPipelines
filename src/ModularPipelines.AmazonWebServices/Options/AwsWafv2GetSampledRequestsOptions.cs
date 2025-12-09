using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "get-sampled-requests")]
public record AwsWafv2GetSampledRequestsOptions(
[property: CliOption("--web-acl-arn")] string WebAclArn,
[property: CliOption("--rule-metric-name")] string RuleMetricName,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--time-window")] string TimeWindow,
[property: CliOption("--max-items")] long MaxItems
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}