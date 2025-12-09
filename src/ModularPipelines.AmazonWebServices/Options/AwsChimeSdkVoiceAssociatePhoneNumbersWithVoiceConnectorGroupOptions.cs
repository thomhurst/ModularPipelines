using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "associate-phone-numbers-with-voice-connector-group")]
public record AwsChimeSdkVoiceAssociatePhoneNumbersWithVoiceConnectorGroupOptions(
[property: CliOption("--voice-connector-group-id")] string VoiceConnectorGroupId,
[property: CliOption("--e164-phone-numbers")] string[] E164PhoneNumbers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}