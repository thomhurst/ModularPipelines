using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "create-archive")]
public record AwsEventsCreateArchiveOptions(
[property: CliOption("--archive-name")] string ArchiveName,
[property: CliOption("--event-source-arn")] string EventSourceArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--event-pattern")]
    public string? EventPattern { get; set; }

    [CliOption("--retention-days")]
    public int? RetentionDays { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}