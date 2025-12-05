using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "delete-voice-connector")]
public record AwsChimeSdkVoiceDeleteVoiceConnectorOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}