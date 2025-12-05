using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "disassociate-phone-numbers-from-voice-connector")]
public record AwsChimeSdkVoiceDisassociatePhoneNumbersFromVoiceConnectorOptions(
[property: CliOption("--voice-connector-id")] string VoiceConnectorId,
[property: CliOption("--e164-phone-numbers")] string[] E164PhoneNumbers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}