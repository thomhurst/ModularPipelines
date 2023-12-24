using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "update-scheduled-action")]
public record AwsOpensearchUpdateScheduledActionOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--action-id")] string ActionId,
[property: CommandSwitch("--action-type")] string ActionType,
[property: CommandSwitch("--schedule-at")] string ScheduleAt
) : AwsOptions
{
    [CommandSwitch("--desired-start-time")]
    public long? DesiredStartTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}