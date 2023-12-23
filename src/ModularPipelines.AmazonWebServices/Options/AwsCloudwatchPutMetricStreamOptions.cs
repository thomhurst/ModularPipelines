using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "put-metric-stream")]
public record AwsCloudwatchPutMetricStreamOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--firehose-arn")] string FirehoseArn,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--output-format")] string OutputFormat
) : AwsOptions
{
    [CommandSwitch("--include-filters")]
    public string[]? IncludeFilters { get; set; }

    [CommandSwitch("--exclude-filters")]
    public string[]? ExcludeFilters { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--statistics-configurations")]
    public string[]? StatisticsConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}