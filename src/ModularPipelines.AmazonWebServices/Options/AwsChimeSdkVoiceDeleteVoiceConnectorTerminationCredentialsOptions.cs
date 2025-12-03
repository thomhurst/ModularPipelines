using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "delete-voice-connector-termination-credentials")]
public record AwsChimeSdkVoiceDeleteVoiceConnectorTerminationCredentialsOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--usernames")] string[] Usernames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}