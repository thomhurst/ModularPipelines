using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "test-alarm")]
public record AwsLightsailTestAlarmOptions(
[property: CommandSwitch("--alarm-name")] string AlarmName,
[property: CommandSwitch("--state")] string State
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}