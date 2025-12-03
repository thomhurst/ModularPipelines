using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-global-settings")]
public record AwsChimeSdkVoiceUpdateGlobalSettingsOptions : AwsOptions
{
    [CliOption("--voice-connector")]
    public string? VoiceConnector { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}