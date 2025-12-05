using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "list-crawls")]
public record AwsGlueListCrawlsOptions(
[property: CliOption("--crawler-name")] string CrawlerName
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}