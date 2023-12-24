using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "get-phone-number-order")]
public record AwsChimeSdkVoiceGetPhoneNumberOrderOptions(
[property: CommandSwitch("--phone-number-order-id")] string PhoneNumberOrderId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}