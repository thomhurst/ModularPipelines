using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-tags")]
public record AwsCeGetTagsOptions(
[property: CliOption("--time-period")] string TimePeriod
) : AwsOptions
{
    [CliOption("--search-string")]
    public string? SearchString { get; set; }

    [CliOption("--tag-key")]
    public string? TagKey { get; set; }

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