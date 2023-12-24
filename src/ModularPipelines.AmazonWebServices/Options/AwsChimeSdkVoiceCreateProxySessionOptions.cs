using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "create-proxy-session")]
public record AwsChimeSdkVoiceCreateProxySessionOptions(
[property: CommandSwitch("--voice-connector-id")] string VoiceConnectorId,
[property: CommandSwitch("--participant-phone-numbers")] string[] ParticipantPhoneNumbers,
[property: CommandSwitch("--capabilities")] string[] Capabilities
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--expiry-minutes")]
    public int? ExpiryMinutes { get; set; }

    [CommandSwitch("--number-selection-behavior")]
    public string? NumberSelectionBehavior { get; set; }

    [CommandSwitch("--geo-match-level")]
    public string? GeoMatchLevel { get; set; }

    [CommandSwitch("--geo-match-params")]
    public string? GeoMatchParams { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}