using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-crawler-schedule")]
public record AwsGlueUpdateCrawlerScheduleOptions(
[property: CliOption("--crawler-name")] string CrawlerName
) : AwsOptions
{
    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}