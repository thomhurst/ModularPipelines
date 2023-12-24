using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "create-archive")]
public record AwsEventsCreateArchiveOptions(
[property: CommandSwitch("--archive-name")] string ArchiveName,
[property: CommandSwitch("--event-source-arn")] string EventSourceArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--event-pattern")]
    public string? EventPattern { get; set; }

    [CommandSwitch("--retention-days")]
    public int? RetentionDays { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}