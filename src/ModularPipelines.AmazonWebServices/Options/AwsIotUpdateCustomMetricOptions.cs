using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-custom-metric")]
public record AwsIotUpdateCustomMetricOptions(
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--display-name")] string DisplayName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}