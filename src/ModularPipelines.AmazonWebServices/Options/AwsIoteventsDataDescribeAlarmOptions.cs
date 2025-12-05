using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents-data", "describe-alarm")]
public record AwsIoteventsDataDescribeAlarmOptions(
[property: CliOption("--alarm-model-name")] string AlarmModelName
) : AwsOptions
{
    [CliOption("--key-value")]
    public string? KeyValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}