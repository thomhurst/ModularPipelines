using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "delete-metric-attribution")]
public record AwsPersonalizeDeleteMetricAttributionOptions(
[property: CommandSwitch("--metric-attribution-arn")] string MetricAttributionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}