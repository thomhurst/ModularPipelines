using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-metric-filter")]
public record AwsLogsPutMetricFilterOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName,
[property: CommandSwitch("--filter-name")] string FilterName,
[property: CommandSwitch("--filter-pattern")] string FilterPattern,
[property: CommandSwitch("--metric-transformations")] string[] MetricTransformations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}