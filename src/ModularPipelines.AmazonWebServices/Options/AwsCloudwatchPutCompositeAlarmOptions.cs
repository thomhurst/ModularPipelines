using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-composite-alarm")]
public record AwsCloudwatchPutCompositeAlarmOptions(
[property: CliOption("--alarm-name")] string AlarmName,
[property: CliOption("--alarm-rule")] string AlarmRule
) : AwsOptions
{
    [CliOption("--alarm-actions")]
    public string[]? AlarmActions { get; set; }

    [CliOption("--alarm-description")]
    public string? AlarmDescription { get; set; }

    [CliOption("--insufficient-data-actions")]
    public string[]? InsufficientDataActions { get; set; }

    [CliOption("--ok-actions")]
    public string[]? OkActions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--actions-suppressor")]
    public string? ActionsSuppressor { get; set; }

    [CliOption("--actions-suppressor-wait-period")]
    public int? ActionsSuppressorWaitPeriod { get; set; }

    [CliOption("--actions-suppressor-extension-period")]
    public int? ActionsSuppressorExtensionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}