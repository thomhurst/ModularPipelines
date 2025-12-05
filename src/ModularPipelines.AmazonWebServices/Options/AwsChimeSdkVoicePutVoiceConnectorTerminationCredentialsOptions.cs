using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "put-voice-connector-termination-credentials")]
public record AwsChimeSdkVoicePutVoiceConnectorTerminationCredentialsOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId
) : AwsOptions
{
    [CliOption("--credentials")]
    public string[]? Credentials { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}