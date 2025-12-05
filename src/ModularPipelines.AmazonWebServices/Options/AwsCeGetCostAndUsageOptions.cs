using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-cost-and-usage")]
public record AwsCeGetCostAndUsageOptions(
[property: CliOption("--time-period")] string TimePeriod,
[property: CliOption("--granularity")] string Granularity,
[property: CliOption("--metrics")] string[] Metrics
) : AwsOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--group-by")]
    public string[]? GroupBy { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}