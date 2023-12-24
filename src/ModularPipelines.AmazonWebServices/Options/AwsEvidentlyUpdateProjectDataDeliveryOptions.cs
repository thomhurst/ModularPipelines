using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "update-project-data-delivery")]
public record AwsEvidentlyUpdateProjectDataDeliveryOptions(
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--cloud-watch-logs")]
    public string? CloudWatchLogs { get; set; }

    [CommandSwitch("--s3-destination")]
    public string? S3Destination { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}