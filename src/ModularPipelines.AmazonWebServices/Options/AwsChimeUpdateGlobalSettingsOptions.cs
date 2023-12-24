using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "update-global-settings")]
public record AwsChimeUpdateGlobalSettingsOptions : AwsOptions
{
    [CommandSwitch("--business-calling")]
    public string? BusinessCalling { get; set; }

    [CommandSwitch("--voice-connector")]
    public string? VoiceConnector { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}