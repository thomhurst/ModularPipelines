using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "list-crawls")]
public record AwsGlueListCrawlsOptions(
[property: CommandSwitch("--crawler-name")] string CrawlerName
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}