using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "update-project-data-delivery")]
public record AwsEvidentlyUpdateProjectDataDeliveryOptions(
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--cloud-watch-logs")]
    public string? CloudWatchLogs { get; set; }

    [CliOption("--s3-destination")]
    public string? S3Destination { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}