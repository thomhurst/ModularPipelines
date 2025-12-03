using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-voice-connector-logging-configuration")]
public record AwsChimeSdkVoicePutVoiceConnectorLoggingConfigurationOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--logging-configuration")] string LoggingConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}