using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "disassociate-phone-numbers-from-voice-connector-group")]
public record AwsChimeSdkVoiceDisassociatePhoneNumbersFromVoiceConnectorGroupOptions(
[property: CommandSwitch("--voice-connector-group-id")] string VoiceConnectorGroupId,
[property: CommandSwitch("--e164-phone-numbers")] string[] E164PhoneNumbers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}