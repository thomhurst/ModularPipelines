using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-metric-attribution")]
public record AwsPersonalizeCreateMetricAttributionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn,
[property: CliOption("--metrics")] string[] Metrics,
[property: CliOption("--metrics-output-config")] string MetricsOutputConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}