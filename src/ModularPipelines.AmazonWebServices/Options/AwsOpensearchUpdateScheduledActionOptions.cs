using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "update-scheduled-action")]
public record AwsOpensearchUpdateScheduledActionOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--action-id")] string ActionId,
[property: CliOption("--action-type")] string ActionType,
[property: CliOption("--schedule-at")] string ScheduleAt
) : AwsOptions
{
    [CliOption("--desired-start-time")]
    public long? DesiredStartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}