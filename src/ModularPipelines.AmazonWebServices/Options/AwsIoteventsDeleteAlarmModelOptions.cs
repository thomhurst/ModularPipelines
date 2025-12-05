using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "delete-alarm-model")]
public record AwsIoteventsDeleteAlarmModelOptions(
[property: CliOption("--alarm-model-name")] string AlarmModelName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}