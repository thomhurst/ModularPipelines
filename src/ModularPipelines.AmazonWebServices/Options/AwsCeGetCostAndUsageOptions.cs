using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-cost-and-usage")]
public record AwsCeGetCostAndUsageOptions(
[property: CommandSwitch("--time-period")] string TimePeriod,
[property: CommandSwitch("--granularity")] string Granularity,
[property: CommandSwitch("--metrics")] string[] Metrics
) : AwsOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--group-by")]
    public string[]? GroupBy { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}