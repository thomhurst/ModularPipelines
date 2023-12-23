using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-metric-attribution")]
public record AwsPersonalizeCreateMetricAttributionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn,
[property: CommandSwitch("--metrics")] string[] Metrics,
[property: CommandSwitch("--metrics-output-config")] string MetricsOutputConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}