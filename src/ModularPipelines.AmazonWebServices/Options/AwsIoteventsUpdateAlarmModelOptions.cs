using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "update-alarm-model")]
public record AwsIoteventsUpdateAlarmModelOptions(
[property: CliOption("--alarm-model-name")] string AlarmModelName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--alarm-rule")] string AlarmRule
) : AwsOptions
{
    [CliOption("--alarm-model-description")]
    public string? AlarmModelDescription { get; set; }

    [CliOption("--severity")]
    public int? Severity { get; set; }

    [CliOption("--alarm-notification")]
    public string? AlarmNotification { get; set; }

    [CliOption("--alarm-event-actions")]
    public string? AlarmEventActions { get; set; }

    [CliOption("--alarm-capabilities")]
    public string? AlarmCapabilities { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}