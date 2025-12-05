using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-configuration-recorder")]
public record AwsConfigservicePutConfigurationRecorderOptions(
[property: CliOption("--configuration-recorder")] string ConfigurationRecorder
) : AwsOptions
{
    [CliOption("--recording-group")]
    public string? RecordingGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}