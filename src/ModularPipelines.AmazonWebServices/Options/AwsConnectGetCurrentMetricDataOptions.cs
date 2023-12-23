using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "get-current-metric-data")]
public record AwsConnectGetCurrentMetricDataOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--filters")] string Filters,
[property: CommandSwitch("--current-metrics")] string[] CurrentMetrics
) : AwsOptions
{
    [CommandSwitch("--groupings")]
    public string[]? Groupings { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--sort-criteria")]
    public string[]? SortCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}