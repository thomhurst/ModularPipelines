using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-voice-connector-group")]
public record AwsChimeSdkVoiceUpdateVoiceConnectorGroupOptions(
[property: CliOption("--voice-connector-group-id")] string VoiceConnectorGroupId,
[property: CliOption("--name")] string Name,
[property: CliOption("--voice-connector-items")] string[] VoiceConnectorItems
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}