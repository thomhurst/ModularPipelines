using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-cost-and-usage-with-resources")]
public record AwsCeGetCostAndUsageWithResourcesOptions(
[property: CommandSwitch("--time-period")] string TimePeriod,
[property: CommandSwitch("--granularity")] string Granularity,
[property: CommandSwitch("--filter")] string Filter
) : AwsOptions
{
    [CommandSwitch("--metrics")]
    public string[]? Metrics { get; set; }

    [CommandSwitch("--group-by")]
    public string[]? GroupBy { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}