using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "get-phone-number-order")]
public record AwsChimeSdkVoiceGetPhoneNumberOrderOptions(
[property: CliOption("--phone-number-order-id")] string PhoneNumberOrderId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}