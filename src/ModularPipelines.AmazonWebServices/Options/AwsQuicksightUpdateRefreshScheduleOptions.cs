using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-refresh-schedule")]
public record AwsQuicksightUpdateRefreshScheduleOptions(
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--schedule")] string Schedule
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}