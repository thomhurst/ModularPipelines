using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "create-alarm-model")]
public record AwsIoteventsCreateAlarmModelOptions(
[property: CliOption("--alarm-model-name")] string AlarmModelName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--alarm-rule")] string AlarmRule
) : AwsOptions
{
    [CliOption("--alarm-model-description")]
    public string? AlarmModelDescription { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

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