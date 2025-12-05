using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "get-voice-connector-group")]
public record AwsChimeSdkVoiceGetVoiceConnectorGroupOptions(
[property: CliOption("--voice-connector-group-id")] string VoiceConnectorGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}