using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents", "update-alarm-model")]
public record AwsIoteventsUpdateAlarmModelOptions(
[property: CommandSwitch("--alarm-model-name")] string AlarmModelName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--alarm-rule")] string AlarmRule
) : AwsOptions
{
    [CommandSwitch("--alarm-model-description")]
    public string? AlarmModelDescription { get; set; }

    [CommandSwitch("--severity")]
    public int? Severity { get; set; }

    [CommandSwitch("--alarm-notification")]
    public string? AlarmNotification { get; set; }

    [CommandSwitch("--alarm-event-actions")]
    public string? AlarmEventActions { get; set; }

    [CommandSwitch("--alarm-capabilities")]
    public string? AlarmCapabilities { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}