using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "update-archive")]
public record AwsEventsUpdateArchiveOptions(
[property: CliOption("--archive-name")] string ArchiveName
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