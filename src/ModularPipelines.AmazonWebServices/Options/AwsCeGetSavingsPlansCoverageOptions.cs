using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-savings-plans-coverage")]
public record AwsCeGetSavingsPlansCoverageOptions(
[property: CliOption("--time-period")] string TimePeriod
) : AwsOptions
{
    [CliOption("--group-by")]
    public string[]? GroupBy { get; set; }

    [CliOption("--granularity")]
    public string? Granularity { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--metrics")]
    public string[]? Metrics { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}