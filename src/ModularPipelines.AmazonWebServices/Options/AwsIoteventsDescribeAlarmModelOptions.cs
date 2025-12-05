using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "describe-alarm-model")]
public record AwsIoteventsDescribeAlarmModelOptions(
[property: CliOption("--alarm-model-name")] string AlarmModelName
) : AwsOptions
{
    [CliOption("--alarm-model-version")]
    public string? AlarmModelVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}