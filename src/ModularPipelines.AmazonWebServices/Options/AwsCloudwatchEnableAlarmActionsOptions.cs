using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "enable-alarm-actions")]
public record AwsCloudwatchEnableAlarmActionsOptions(
[property: CliOption("--alarm-names")] string[] AlarmNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}