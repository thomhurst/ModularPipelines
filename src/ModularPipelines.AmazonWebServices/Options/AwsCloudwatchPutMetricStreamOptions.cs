using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-metric-stream")]
public record AwsCloudwatchPutMetricStreamOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--firehose-arn")] string FirehoseArn,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--output-format")] string OutputFormat
) : AwsOptions
{
    [CliOption("--include-filters")]
    public string[]? IncludeFilters { get; set; }

    [CliOption("--exclude-filters")]
    public string[]? ExcludeFilters { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--statistics-configurations")]
    public string[]? StatisticsConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}