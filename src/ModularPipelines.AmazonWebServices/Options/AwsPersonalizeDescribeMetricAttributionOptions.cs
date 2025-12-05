using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "describe-metric-attribution")]
public record AwsPersonalizeDescribeMetricAttributionOptions(
[property: CliOption("--metric-attribution-arn")] string MetricAttributionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}