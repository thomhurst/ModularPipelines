using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-savings-plans-utilization-details")]
public record AwsCeGetSavingsPlansUtilizationDetailsOptions(
[property: CliOption("--time-period")] string TimePeriod
) : AwsOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--data-type")]
    public string[]? DataType { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}