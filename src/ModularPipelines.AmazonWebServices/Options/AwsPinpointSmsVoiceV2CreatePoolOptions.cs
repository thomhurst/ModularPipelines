using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "create-pool")]
public record AwsPinpointSmsVoiceV2CreatePoolOptions(
[property: CliOption("--origination-identity")] string OriginationIdentity,
[property: CliOption("--iso-country-code")] string IsoCountryCode,
[property: CliOption("--message-type")] string MessageType
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}