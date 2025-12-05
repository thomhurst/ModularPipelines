using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "associate-origination-identity")]
public record AwsPinpointSmsVoiceV2AssociateOriginationIdentityOptions(
[property: CliOption("--pool-id")] string PoolId,
[property: CliOption("--origination-identity")] string OriginationIdentity,
[property: CliOption("--iso-country-code")] string IsoCountryCode
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}