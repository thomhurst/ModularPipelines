using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "delete-refresh-schedule")]
public record AwsQuicksightDeleteRefreshScheduleOptions(
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--schedule-id")] string ScheduleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}