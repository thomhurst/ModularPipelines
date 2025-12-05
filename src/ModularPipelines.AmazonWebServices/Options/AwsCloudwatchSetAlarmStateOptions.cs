using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "set-alarm-state")]
public record AwsCloudwatchSetAlarmStateOptions(
[property: CliOption("--alarm-name")] string AlarmName,
[property: CliOption("--state-value")] string StateValue,
[property: CliOption("--state-reason")] string StateReason
) : AwsOptions
{
    [CliOption("--state-reason-data")]
    public string? StateReasonData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}