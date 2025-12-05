using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "create-voice-connector-group")]
public record AwsChimeSdkVoiceCreateVoiceConnectorGroupOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--voice-connector-items")]
    public string[]? VoiceConnectorItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}