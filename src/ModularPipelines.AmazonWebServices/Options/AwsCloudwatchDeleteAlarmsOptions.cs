using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "delete-alarms")]
public record AwsCloudwatchDeleteAlarmsOptions(
[property: CommandSwitch("--alarm-names")] string[] AlarmNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}