using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "update-phone-number")]
public record AwsChimeSdkVoiceUpdatePhoneNumberOptions(
[property: CliOption("--phone-number-id")] string PhoneNumberId
) : AwsOptions
{
    [CliOption("--product-type")]
    public string? ProductType { get; set; }

    [CliOption("--calling-name")]
    public string? CallingName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}