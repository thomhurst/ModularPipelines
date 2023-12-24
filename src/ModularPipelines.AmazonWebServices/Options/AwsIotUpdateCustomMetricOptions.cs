using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-custom-metric")]
public record AwsIotUpdateCustomMetricOptions(
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--display-name")] string DisplayName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}