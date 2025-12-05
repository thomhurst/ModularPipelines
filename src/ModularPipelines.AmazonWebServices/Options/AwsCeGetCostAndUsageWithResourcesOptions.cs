using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-cost-and-usage-with-resources")]
public record AwsCeGetCostAndUsageWithResourcesOptions(
[property: CliOption("--time-period")] string TimePeriod,
[property: CliOption("--granularity")] string Granularity,
[property: CliOption("--filter")] string Filter
) : AwsOptions
{
    [CliOption("--metrics")]
    public string[]? Metrics { get; set; }

    [CliOption("--group-by")]
    public string[]? GroupBy { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}