using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "search-index")]
public record AwsIotSearchIndexOptions(
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--query-version")]
    public string? QueryVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}