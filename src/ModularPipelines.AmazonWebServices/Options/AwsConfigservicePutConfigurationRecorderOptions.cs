using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-configuration-recorder")]
public record AwsConfigservicePutConfigurationRecorderOptions(
[property: CommandSwitch("--configuration-recorder")] string ConfigurationRecorder
) : AwsOptions
{
    [CommandSwitch("--recording-group")]
    public string? RecordingGroup { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}