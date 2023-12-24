using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "put-composite-alarm")]
public record AwsCloudwatchPutCompositeAlarmOptions(
[property: CommandSwitch("--alarm-name")] string AlarmName,
[property: CommandSwitch("--alarm-rule")] string AlarmRule
) : AwsOptions
{
    [CommandSwitch("--alarm-actions")]
    public string[]? AlarmActions { get; set; }

    [CommandSwitch("--alarm-description")]
    public string? AlarmDescription { get; set; }

    [CommandSwitch("--insufficient-data-actions")]
    public string[]? InsufficientDataActions { get; set; }

    [CommandSwitch("--ok-actions")]
    public string[]? OkActions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--actions-suppressor")]
    public string? ActionsSuppressor { get; set; }

    [CommandSwitch("--actions-suppressor-wait-period")]
    public int? ActionsSuppressorWaitPeriod { get; set; }

    [CommandSwitch("--actions-suppressor-extension-period")]
    public int? ActionsSuppressorExtensionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}