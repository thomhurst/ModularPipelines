using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "set-alarm-state")]
public record AwsCloudwatchSetAlarmStateOptions(
[property: CommandSwitch("--alarm-name")] string AlarmName,
[property: CommandSwitch("--state-value")] string StateValue,
[property: CommandSwitch("--state-reason")] string StateReason
) : AwsOptions
{
    [CommandSwitch("--state-reason-data")]
    public string? StateReasonData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}