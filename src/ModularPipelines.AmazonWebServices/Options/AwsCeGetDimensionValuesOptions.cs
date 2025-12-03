using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-dimension-values")]
public record AwsCeGetDimensionValuesOptions(
[property: CliOption("--time-period")] string TimePeriod,
[property: CliOption("--dimension")] string Dimension
) : AwsOptions
{
    [CliOption("--search-string")]
    public string? SearchString { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--sort-by")]
    public string[]? SortBy { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}