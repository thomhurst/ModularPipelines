using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "update-global-settings")]
public record AwsChimeSdkVoiceUpdateGlobalSettingsOptions : AwsOptions
{
    [CommandSwitch("--voice-connector")]
    public string? VoiceConnector { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}