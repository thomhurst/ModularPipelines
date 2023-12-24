using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "put-voice-connector-logging-configuration")]
public record AwsChimeSdkVoicePutVoiceConnectorLoggingConfigurationOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId,
[property: CommandSwitch("--logging-configuration")] string LoggingConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}