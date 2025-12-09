using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "test-alarm")]
public record AwsLightsailTestAlarmOptions(
[property: CliOption("--alarm-name")] string AlarmName,
[property: CliOption("--state")] string State
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}