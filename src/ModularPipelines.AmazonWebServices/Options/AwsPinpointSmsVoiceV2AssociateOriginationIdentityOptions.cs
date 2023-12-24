using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "associate-origination-identity")]
public record AwsPinpointSmsVoiceV2AssociateOriginationIdentityOptions(
[property: CommandSwitch("--pool-id")] string PoolId,
[property: CommandSwitch("--origination-identity")] string OriginationIdentity,
[property: CommandSwitch("--iso-country-code")] string IsoCountryCode
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}