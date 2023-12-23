using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-crawler-schedule")]
public record AwsGlueUpdateCrawlerScheduleOptions(
[property: CommandSwitch("--crawler-name")] string CrawlerName
) : AwsOptions
{
    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}