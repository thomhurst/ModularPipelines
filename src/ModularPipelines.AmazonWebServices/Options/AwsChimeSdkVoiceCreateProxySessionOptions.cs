using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "create-proxy-session")]
public record AwsChimeSdkVoiceCreateProxySessionOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--participant-phone-numbers")] string[] ParticipantPhoneNumbers,
[property: CliOption("--capabilities")] string[] Capabilities
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--expiry-minutes")]
    public int? ExpiryMinutes { get; set; }

    [CliOption("--number-selection-behavior")]
    public string? NumberSelectionBehavior { get; set; }

    [CliOption("--geo-match-level")]
    public string? GeoMatchLevel { get; set; }

    [CliOption("--geo-match-params")]
    public string? GeoMatchParams { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}